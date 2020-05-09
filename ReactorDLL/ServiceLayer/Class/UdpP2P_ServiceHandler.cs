using Reactor.Interface;
using System.Net;
using Reactor.Utilities;

namespace ServiceLayer.Class
{
    public class UdpP2P_ServiceHandler : UdpServiceHandlerBase
    {
        public UdpP2P_ServiceHandler(IReactor reactor, EndPoint localEndpoint, EndPoint remoteEndpoint) 
                                    : base(reactor, localEndpoint, remoteEndpoint)
        {

        }


        public override void Open()
        {
            this.Handle = SocketFactory.CreateNonBlockingUdpP2PSocket();
            this.Handle.Bind(local);
            IPEndPoint ipEp = new IPEndPoint(((IPEndPoint)remote).Address, ((IPEndPoint)remote).Port);
            this.Handle.Connect(ipEp);
            this.reactor.RegisterHandler(this, EventType.READ_WRITE_EVENT);
        }
    }
}
