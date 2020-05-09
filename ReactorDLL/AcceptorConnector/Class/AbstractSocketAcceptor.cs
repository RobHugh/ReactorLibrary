using AcceptorConnector.Interface;
using Reactor.Utilities;
using Reactor.Interface;
using slf4net;
using System;
using System.Net;
using System.Net.Sockets;

namespace AcceptorConnector.Class
{
    abstract public class AbstractSocketAcceptor : IAcceptor
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(AbstractSocketAcceptor));
        private ISocket handle;
        private IServiceHandler serviceHandler;
        protected IReactor reactor;

        public Action AcceptNotification { get; set;  }

        public ISocket Handle
        {
            get { return handle; }
            set
            {
                throw new NotImplementedException("Setting Socket Handle for Acceptor not implemented. Should be set in constructor.");
            }
        }
        public IServiceHandler ServiceHandler
        {
            get { return serviceHandler; }
            set
            {
                throw new NotImplementedException("Setting of service handler is done through internal Accept Method.");
            }
        }

        public AbstractSocketAcceptor(EndPoint localEndPoint, 
                                    IReactor reactor, 
                                    int pendingConnections)
        {
            this.reactor = reactor;     
            Open(localEndPoint, pendingConnections);
        }

        virtual public void Accept()
        {
            serviceHandler = MakeServiceHandler();
            AcceptServiceHandler(serviceHandler);
            ActivateServiceHandler(serviceHandler);
        }

        virtual public void HandleEvent(ISocket handle, EventType eventType)
        {
            switch(eventType)
            {
                case EventType.ACCEPT_EVENT:
                    {
                        log.Info("Acceptor ACCEPT_EVENT handled");
                        Accept();
                        break;
                    }
                default:
                    {
                        log.Info(String.Format("Acceptor {0} ignored", eventType.ToString()));
                        break;
                    }
            }
        }

        virtual public void Dispose()
        {
            this.reactor?.RemoveHandler(this, EventType.ACCEPT_EVENT);
            handle?.Dispose();        
            log.Info("Acceptor resources disposed");
        }

        private void Open(EndPoint localEndPoint, int pendingConnections)
        {
            try
            {
                handle = SocketFactory.CreateNonBlockingTcpSocket();
                IPEndPoint ipep = (IPEndPoint)localEndPoint;
	            handle.UserSocket.ExclusiveAddressUse = false;
	            handle.Bind(ipep);
	            handle.Listen(pendingConnections);
                this.reactor.RegisterHandler(this, EventType.ACCEPT_EVENT);
                log.Info("Acceptor listening on {0} : {1}", ipep.Address.ToString(), ipep.Port.ToString());
            }
            catch(ObjectDisposedException ex)
            {
                log.Warn(ex, "Acceptor Socket has been disposed");
            }
            catch (SocketException ex)
            {
                log.Error(ex, String.Format("Acceptor failed to bind to local endpoint. WSAError - {0}", 
                                             ex.SocketErrorCode.ToString()));
                Dispose();
            }
        }

        abstract protected IServiceHandler MakeServiceHandler();

        private void AcceptServiceHandler(IServiceHandler serviceHandler)
        {
            serviceHandler.Handle = handle.Accept();
        }

        private void ActivateServiceHandler(IServiceHandler serviceHandler)
        {
            if(serviceHandler.Handle.UserSocket.Connected)
            {
                log.Info("ServiceHandler connected.");
                serviceHandler.Open();
                AcceptNotification?.Invoke();
            }
            else
            {
                log.Warn("ServiceHandler not connected.");
                serviceHandler.Dispose();
                serviceHandler = null;
            }
        }
    }
}
