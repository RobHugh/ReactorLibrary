using Reactor.Interface;
using Reactor.Utilities;
using System.Net;

namespace ServiceLayer.Class
{
    public class UdpBroadcast_ServiceHandler : UdpServiceHandlerBase
    {
        public UdpBroadcast_ServiceHandler(IReactor reactor, EndPoint localEndpoint, EndPoint remoteEndpoint)
                                    : base(reactor, localEndpoint, remoteEndpoint)
        {

        }


        public override void Open()
        {
            this.Handle = SocketFactory.CreateNonBlockingUdpBroadcastSocket();
            this.Handle.Bind(local);
            IPEndPoint ipEp = new IPEndPoint(IPAddress.Broadcast, ((IPEndPoint)local).Port);
            this.Handle.Connect(ipEp);
            this.reactor.RegisterHandler(this, EventType.READ_WRITE_EVENT);
        }
    }
}
