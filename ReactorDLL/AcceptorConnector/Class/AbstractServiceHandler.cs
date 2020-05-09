using System;
using AcceptorConnector.Interface;
using Reactor.Interface;
using System.Net;
using slf4net;

namespace AcceptorConnector.Class
{
    public abstract class AbstractServiceHandler : IServiceHandler
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(AbstractServiceHandler));
        protected IReactor reactor;
        protected byte[] readData;
        protected byte[] writeData;

        public ISocket Handle { get; set; }
        public EndPoint RemoteURI { get { return Handle.RemoteEndPoint; }
                                    set { throw new NotImplementedException("RemoteEndpoint cannot be set for service handler"); } }

        public byte[] ReadData
        {
            get
            {
                return readData;
            }
        }

        public byte[] WriteData
        {
            set
            {
                writeData = value;
            }
        }

        public Action<IServiceHandler> ReadCompleteNotification { get; set; }


        public Action<IServiceHandler> WriteCompleteNotification { get; set; }

        public Action<IServiceHandler> CloseEventNotification { get; set; }

        public Action<IServiceHandler, ConnectionResult> ConnectionNotification { get; set; }

        public Action<IServiceHandler,Exception> ErrorNotification { get; set; }

        public AbstractServiceHandler(IReactor reactor)
        {
            this.reactor = reactor;
        }

        abstract public void HandleEvent(ISocket handle, EventType eventType);

        abstract public void Open();

        virtual public void Dispose()
        {
            reactor?.RemoveHandler(this, EventType.READ_EVENT & EventType.WRITE_EVENT);
            Handle?.Dispose();
            log.Info("SocketServiceHandler disposed");
        }
    }
}
