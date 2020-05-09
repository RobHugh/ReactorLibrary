using System.Net.Sockets;

namespace Reactor.Class
{
    class NonBlockingUdpP2PImpl : UdpSocketBase
    {
        public NonBlockingUdpP2PImpl() : base()
        {
            UserSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            UserSocket.ExclusiveAddressUse = false;
            UserSocket.Blocking = false;
        }
    }
}
