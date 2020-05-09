using System.Net.Sockets;

namespace Reactor.Class
{
    class NonBlockingUdpBroadcastImpl : UdpSocketBase
    {
        public NonBlockingUdpBroadcastImpl() : base()
        {
            UserSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            UserSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            UserSocket.ExclusiveAddressUse = false;
            UserSocket.Blocking = false;
        }
    }
}
