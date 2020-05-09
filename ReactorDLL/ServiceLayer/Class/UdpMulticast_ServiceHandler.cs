using Reactor.Interface;
using Reactor.Utilities;
using System.Net;

namespace ServiceLayer.Class
{
    public class UdpMulticast_ServiceHandler : UdpServiceHandlerBase
    {
        private bool multicastLoopback;
        private int timeToLive;

        public UdpMulticast_ServiceHandler(IReactor reactor, 
                                            EndPoint localEndpoint, 
                                            EndPoint remoteEndpoint, 
                                            bool multicastLoopback, int ttl) 
                                            : base(reactor, localEndpoint, remoteEndpoint)
        {
            this.multicastLoopback = multicastLoopback;
            timeToLive = ttl;
        }

        public override void Open()
        {
            this.Handle = SocketFactory.CreateNonBlockingUdpMulticastSocket(remote, local, multicastLoopback, timeToLive);
            this.Handle.Bind(local);
            IPEndPoint ipEp = new IPEndPoint(((IPEndPoint)remote).Address, ((IPEndPoint)remote).Port);
            this.Handle.Connect(ipEp);
            this.reactor.RegisterHandler(this, EventType.READ_WRITE_EVENT);
        }
    }
}
