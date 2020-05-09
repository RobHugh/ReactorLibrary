using AcceptorConnector.Class;
using Reactor.Interface;
using slf4net;

namespace ServiceLayer.Class
{
    public class Tcp_SocketConnector : AbstractSocketConnector
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(Tcp_SocketConnector));
        public Tcp_SocketConnector(IReactor reactor) : base(reactor)
        {
        }
    }
}
