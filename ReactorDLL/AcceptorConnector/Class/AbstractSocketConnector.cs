using AcceptorConnector.Interface;
using Reactor.Interface;
using slf4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace AcceptorConnector.Class
{
    public abstract class AbstractSocketConnector : IConnector
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(AbstractSocketConnector));
        private IReactor reactor;
        private Dictionary<ISocket, IServiceHandler> connectionMap;


        public AbstractSocketConnector(IReactor reactor)
        {
            this.reactor = reactor;
            connectionMap = new Dictionary<ISocket, IServiceHandler>();
        }

        public virtual void Connect(IServiceHandler serviceHandler, EndPoint remoteEndPoint, Connection_Mode mode)
        {
            ConnectServiceHandler(serviceHandler, remoteEndPoint, mode);
        }

        public virtual void Dispose()
        {
            foreach(var entry in connectionMap)
            {
                reactor?.RemoveHandler(entry.Value, EventType.CONNECT_EVENT);
                entry.Key?.Dispose();
                entry.Value?.Dispose();
            }
        }

        protected virtual void ConnectServiceHandler(IServiceHandler serviceHandler, 
                                                        EndPoint remoteAddress, Connection_Mode mode)
        {
            try
            {
                serviceHandler.Handle.Connect(remoteAddress);
                ActivateServiceHandler(serviceHandler);
            }
            catch(SocketException ex)
            {
                if(ex.SocketErrorCode == SocketError.WouldBlock && mode == Connection_Mode.ASYNC)
                {
                    reactor.RegisterHandler(serviceHandler, EventType.CONNECT_EVENT);
                    serviceHandler.ConnectionNotification += Complete;
                    connectionMap[serviceHandler.Handle] = serviceHandler;
                }
                else
                {
                    log.Error(ex, String.Format("Connector Socket Error has occurred with WSAError - {0}", ex.SocketErrorCode.ToString()));
                    serviceHandler?.Dispose();
                }
            }
        }

        protected virtual void ActivateServiceHandler(IServiceHandler serviceHandler)
        {
            serviceHandler.Open();
        }

        protected virtual void Complete(IServiceHandler handler, ConnectionResult result)
        {
            if(connectionMap.ContainsKey(handler.Handle))
            {
                IServiceHandler serviceHandler = connectionMap[handler.Handle];
                reactor.RemoveHandler(serviceHandler, EventType.CONNECT_EVENT);
                serviceHandler.ConnectionNotification -= Complete;
                connectionMap.Remove(handler.Handle);
                if(result == ConnectionResult.SUCCESS)
                {
                    log.Info("Socket Address {0} connected. Removed from Reactor and Connection map", ((IPEndPoint)handler.Handle.RemoteEndPoint).ToString());
                    ActivateServiceHandler(serviceHandler);
                } 
                else
                {
                    this.Dispose();
                }                              
            }
            else
            {
                log.Warn("Connection map does not contain handle for address {0}", ((IPEndPoint)handler.Handle.RemoteEndPoint).ToString());
            }
        }
    }
}
