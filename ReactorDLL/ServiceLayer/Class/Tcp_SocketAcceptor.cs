using AcceptorConnector.Class;
using AcceptorConnector.Interface;
using ServiceLayer.Interface;
using Reactor.Interface;
using System.Net;
using slf4net;
using System;

namespace ServiceLayer.Class
{
    public class Tcp_SocketAcceptor : AbstractSocketAcceptor
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(Tcp_SocketConnector));
        private bool isConnectionStream;
        private bool serviceKeepAlive;
        private int serviceKeepAliveTimerVal;


        public Tcp_SocketAcceptor(EndPoint localEndPoint, IReactor reactor, int pendingConnections,
                                    bool keepServiceOpen, int keepAliveTimerVal, bool makeStream) 
                                    : base(localEndPoint, reactor, pendingConnections)
        {
            serviceKeepAlive = keepServiceOpen;
            serviceKeepAliveTimerVal = keepAliveTimerVal;
            isConnectionStream = makeStream;
        }
        protected override IServiceHandler MakeServiceHandler()
        {
            log.Info("SocketServiceHandler created");
            if(isConnectionStream)
                return new TcpEoLData_SockServiceHandler(this.reactor);
            else
                return new TcpVarData_SockServiceHandler(this.reactor, serviceKeepAlive, serviceKeepAliveTimerVal);
        }
    }
}
