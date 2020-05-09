using AcceptorConnector.Class;
using System.Net;
using Reactor.Interface;
using System.Net.Sockets;
using slf4net;

namespace ServiceLayer.Class
{

    public abstract class UdpServiceHandlerBase : AbstractServiceHandler
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(UdpServiceHandlerBase));
        protected EndPoint local;
        protected EndPoint remote;

        public UdpServiceHandlerBase(IReactor reactor, EndPoint localEndpoint, EndPoint remoteEndpoint) : base(reactor)
        {
            local = localEndpoint;
            remote = remoteEndpoint;
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
                default:
                    break;
            }
        }

        private void HandleRead()
        {
            int readSize = Handle.UserSocket.Available;
            readData = new byte[readSize];
            try
            {
                int recvSize = Handle.Receive(readData, 0, readSize, SocketFlags.None);
            }
            catch(SocketException se)
            {
                log.Error(se, "Socket Exception Error.");
                ErrorNotification?.Invoke(this,se);
                this.Dispose();
            }
            ReadComplete();
        }

        private void HandleWrite()
        {
            if (writeData != null)
            {
                int writeSize = writeData.Length;
                try
                {
                    int sentSize = Handle.Send(writeData, 0, writeData.Length, SocketFlags.None);
                }
                catch (SocketException se)
                {
                    log.Error(se, "Socket Exception Error.");
                    ErrorNotification?.Invoke(this,se);
                    this.Dispose();
                }
                WriteComplete();
            }
        }

        private void ReadComplete()
        {
            ReadCompleteNotification?.Invoke(this);
            readData = null;
        }

        private void WriteComplete()
        {
            writeData = null;
            WriteCompleteNotification?.Invoke(this);
        }
    }
}
