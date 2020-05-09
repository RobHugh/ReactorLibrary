using System;
using AcceptorConnector.Class;
using AcceptorConnector.Interface;
using Reactor.Interface;
using slf4net;
using System.Net.Sockets;
using System.IO;

namespace ServiceLayer.Class
{
    public class TcpEoLData_SockServiceHandler : AbstractServiceHandler
    {
        private static readonly ILogger log = LoggerFactory.GetLogger(typeof(TcpEoLData_SockServiceHandler));
        private static readonly int END_OF_LINE = 0x0A;

         public TcpEoLData_SockServiceHandler(IReactor reactor) : base(reactor)
        {
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

        public override void Open()
        {
            readData = null;
            writeData = null;
            reactor.RegisterHandler(this, EventType.READ_WRITE_EVENT);
        }

        private void HandleRead()
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // 1 byte buffer
                    // TODO: consider making this variable depending on encoding (currently will only work for UTF8)
                    // ALSO: consider situation of CRLF from unix systems
                    byte[] tempBuffer = new byte[1];
                    tempBuffer[0] = 0x00;
                    while (tempBuffer[0] != END_OF_LINE)
                    {
                        int recvBytes = Handle.Receive(tempBuffer, 0, 1, SocketFlags.None);
                        if (recvBytes == 0)
                        {
                            CloseEventNotification?.Invoke(this);
                            this.Dispose();
                            return;
                        }
                        ms.Write(tempBuffer, 0, 1);
                    }
                    readData = ms.ToArray();
                    ReadComplete();
                }
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10054)
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
            //throw new NotImplementedException();
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
        }


        private void WriteComplete()
        {
            writeData = null;
            WriteCompleteNotification?.Invoke(this);
        }

        private void ReadComplete()
        {
            ReadCompleteNotification?.Invoke(this);
            readData = null;
        }
    }
}
