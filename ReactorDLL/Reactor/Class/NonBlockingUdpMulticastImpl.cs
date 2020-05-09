using System.Net;
using System.Net.Sockets;

namespace Reactor.Class
{
    class NonBlockingUdpMulticastImpl : UdpSocketBase
    {
        private int timeToLive;
        private bool multicastLoopback;
        private EndPoint multicastAddress;
        private EndPoint localAddress;
        public NonBlockingUdpMulticastImpl(EndPoint multicastAddress, EndPoint localAddress, bool loopback, int ttl) : base()
        {
            timeToLive = ttl;
            multicastLoopback = loopback;
            this.localAddress = localAddress;
            this.multicastAddress = multicastAddress;
            UserSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            UserSocket.ExclusiveAddressUse = false;
            UserSocket.Blocking = false;
        }

        public override void Bind(EndPoint ipep)
        {
            base.Bind(ipep);
            MulticastOption multicastOption = new MulticastOption(((IPEndPoint)multicastAddress).Address, ((IPEndPoint)localAddress).Address);
            this.UserSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
            this.UserSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, timeToLive);
            this.UserSocket.MulticastLoopback = multicastLoopback;
        }

        public override void Connect(EndPoint remoteEP)
        {
            base.Connect(remoteEP);
        }

        public override void Dispose()
        {
            MulticastOption multicastOption = new MulticastOption(((IPEndPoint)multicastAddress).Address, ((IPEndPoint)localAddress).Address);
            this.UserSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, multicastOption);
            base.Dispose();
        }
    }
}
