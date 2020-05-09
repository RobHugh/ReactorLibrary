using System;
using AcceptorConnector.Class;
using AcceptorConnector.Interface;
using Reactor.Interface;
using slf4net;
using System.Timers;
using System.Net.Sockets;

namespace ServiceLayer.Class
{
    public class TcpVarData_SockServiceHandler : AbstractServiceHandler
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(TcpVarData_SockServiceHandler));
        private static int HEADER_SIZE = sizeof(int);
        private Timer keepAliveTimer;
        private int keepAliveTimerInterval;
        private double lastReceivedTime;
        private bool keepConnectionAlive;
        private bool recvHeader;
        private bool sentHeader;
        private int dataSent;
        private int dataRecv;
        private int sendSize;
        private int recvSize;
        private int leftToSend;
        private int leftToRecv;        

        public TcpVarData_SockServiceHandler(IReactor reactor, bool keepAlive, int msTimerInterval) : base(reactor)
        {
            keepAliveTimerInterval = msTimerInterval;
            keepConnectionAlive = keepAlive;
            lastReceivedTime = 0;
        }

        public override void Open()
        {
            recvHeader = false;
            sentHeader = false;
            dataSent = 0;
            dataRecv = 0;
            sendSize = 0;
            recvSize = 0;
            leftToSend = 0;
            leftToRecv = 0;
            readData = null;
            writeData = null;
            reactor.RegisterHandler(this, EventType.READ_WRITE_EVENT);
            if(keepConnectionAlive)
            {
                StartTimer(keepAliveTimerInterval);
            }
        }

        public override void HandleEvent(ISocket handle, EventType eventType)
        {
            switch (eventType)
            {
                case EventType.READ_EVENT:
                    {
                        HandleRead();
                        break;
                    }
                case EventType.WRITE_EVENT:
                    {
                        HandleWrite();
                        break;
                    }
                case EventType.CONNECT_EVENT:
                    {
                        HandleConnectionEvent();
                        break;
                    }
                case EventType.ERROR_EVENT:
                    {
                        HandleConnectionError();
                        break;
                    }
                default:
                    break;
            }
        }

        private void HandleRead()
        {
            try
            {
                if(!recvHeader)
                {
                    byte[] headerData = new byte[HEADER_SIZE];
                    int headerSize = headerData.Length;
                    int headerLeftToRecv = headerSize;
                    int headerSizeRecv = 0;

                    while(headerSizeRecv != headerSize)
                    {
                        int recv = Handle.Receive(headerData, headerSizeRecv, headerLeftToRecv, SocketFlags.None);

                        // 0 value for recv means socket has closed from other end
                        if(recv != 0)
                        {
                            headerSizeRecv += recv;
                            headerLeftToRecv -= recv;
                            lastReceivedTime = 0;
                        }
                        else
                        {
                            CloseEventNotification?.Invoke(this);
                            this.Dispose();
                            return;
                        }
                    
                    }

                    int headerDataValue = BitConverter.ToInt32(headerData, 0);
                
                    if (headerDataValue == 0)
                    {
                        // if received a keep alive packet, ignore and exit
                        log.Info("SockServiceHandler keep alive packet received.");
                        return;
                    }
                    else
                    {
                        readData = new byte[headerDataValue];
                        recvSize = headerDataValue;
                        leftToRecv = recvSize;
                        dataRecv = 0;
                        recvHeader = true;
                    }
                    
                }
                else
                {
                    int recv = Handle.Receive(readData, dataRecv, leftToRecv, SocketFlags.None);
                    if(recv == 0)
                    {
                        CloseEventNotification?.Invoke(this);
                        this.Dispose();
                        return;
                    }
                    dataRecv += recv;
                    leftToRecv -= recv;

                    if(dataRecv == recvSize)
                    {
                        ReadComplete();
                    }
                }
            }
            catch(SocketException se)
            {
                if(se.ErrorCode == 10054)
                {
                    // socket connection forcibly closed by remote peer
                    log.Error("Socket Exception Error 10054, socket connection was forcibly closed by the remote peer.");
                    ErrorNotification?.Invoke(this,se);                    
                    this.Dispose();
                }
                else
                {
                    log.Error(se, "Socket Exception Error");
                    ErrorNotification?.Invoke(this,se);                    
                    this.Dispose();
                }
            }
        }

        private void HandleWrite()
        {
            if(writeData != null)
            {
                if(!sentHeader)
                {
                    try
                    {
	                    byte[] header = BitConverter.GetBytes(writeData.Length);
	                    int headerSize = header.Length;
	                    int headerSizeSent = 0;
	                    int headerLeftToSend = headerSize;
	                    
	                    while(headerSizeSent != headerSize)
	                    {
	                        int sent = Handle.Send(header, headerSizeSent, headerLeftToSend, System.Net.Sockets.SocketFlags.None);
	                        headerSizeSent += sent;
	                        headerLeftToSend -= sent;
	                    }

                        ResetKeepAliveTimer();

                        if (writeData.Length > 0)
                        {
	                        sendSize = writeData.Length;
		                    leftToSend = sendSize;
		                    dataSent = 0;
		                    sentHeader = true;
                        }
                        else
                        {
                            // if writeDataLength is 0 it is a keep alive message so reset
                            writeData = null;
                        }
                    }
                    catch (SocketException se)
                    {
                        // deal with remote disconnection event. Raise a close event notification
                        log.Error(se, "Socket Exception Error");
                        ErrorNotification?.Invoke(this,se);
                        this.Dispose();
                    }
                }
                else
                {
                    try
                    {
	                    int sent = Handle.Send(writeData, dataSent, leftToSend, SocketFlags.None);
	                    dataSent += sent;
	                    leftToSend -= sent;
	                    ResetKeepAliveTimer();
	
	                    if (dataSent == sendSize)
	                    {
	                        WriteComplete();
	                    }
                    }
                    catch (SocketException se)
                    {
                        // deal with remote disconnection event. Raise a close event notification
                        log.Error(se, "Socket Exception Error");
                        ErrorNotification?.Invoke(this,se);
                        this.Dispose();
                    }
                }
            }
        }

        private void HandleConnectionError()
        {
            log.Info("Socket could not make a connection to the remote host.");
            ConnectionNotification?.Invoke(this, ConnectionResult.FAIL);
        }

        private void HandleConnectionEvent()
        {
            log.Info("Socket connection made.");
            ConnectionNotification?.Invoke(this, ConnectionResult.SUCCESS);
        }

        public override void Dispose()
        {
            base.Dispose();
            if(keepAliveTimer != null)
            {
                StopTimer();
            }
        }

        private void StartTimer(int millisecondInterval)
        {
            keepAliveTimer = new Timer();
            keepAliveTimer.Elapsed += OnTimerEvent;
            keepAliveTimer.Interval = millisecondInterval;
            keepAliveTimer.Enabled = true;
        }

        private void StopTimer()
        {
            keepAliveTimer.Enabled = false;
            keepAliveTimer.Elapsed -= OnTimerEvent;
            keepAliveTimer.Dispose();
        }

        private void OnTimerEvent(object sender, ElapsedEventArgs args)
        {
	        if (writeData == null)
	        {
	            writeData = new byte[0];
	            log.Info("SockServiceHandler - Keep Alive message queued");
	        }
            Timer tempTimer = sender as Timer;
            lastReceivedTime += tempTimer.Interval;
            if(lastReceivedTime > (2 * tempTimer.Interval))
            {
                var errorString = String.Format("Keep Alive packet not received within {0} ms. Connection closed as invalid.", lastReceivedTime);
                log.Error(errorString);
                ErrorNotification?.Invoke(this,new OutOfTimeException(errorString));
                this.Dispose();
            }
        }

        private void ResetKeepAliveTimer()
        {
            if(keepAliveTimer != null)
                keepAliveTimer.Enabled = true;
        }

        private void WriteComplete()
        {
            sentHeader = false;
            sendSize = 0;
            leftToSend = 0;
            dataSent = 0;
            writeData = null;
            WriteCompleteNotification?.Invoke(this);
        }

        private void ReadComplete()
        {
            recvHeader = false;
            recvSize = 0;
            leftToRecv = 0;
            dataRecv = 0;
            ReadCompleteNotification?.Invoke(this);
            readData = null;
        }
    }

    public class OutOfTimeException : Exception
    {
        public OutOfTimeException(string message) : base(message)
        {

        }
    }
}
