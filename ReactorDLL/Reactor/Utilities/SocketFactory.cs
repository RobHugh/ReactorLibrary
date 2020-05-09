using Reactor.Interface;
using Reactor.Class;
using System.Net;

namespace Reactor.Utilities
{
    public static class SocketFactory
    {
        public static ISocket CreateNonBlockingFromSocket(ISocket socket)
        {
            ISocket socketWrapper = new NonBlockingTcpSocketImpl(socket.UserSocket);
            return socketWrapper;
        }

        public static ISocket CreateNonBlockingTcpSocket()
        {
            ISocket socketWrapper = new NonBlockingTcpSocketImpl();
            return socketWrapper;
        }

        public static ISocket CreateBlockingFromSocket(ISocket socket)
        {
            ISocket socketWrapper = new BlockingTcpSocketImpl(socket.UserSocket);
            return socketWrapper;
        }

        public static ISocket CreateBlockingTcpSocket()
        {
            ISocket socketWrapper = new BlockingTcpSocketImpl();
            return socketWrapper;
        }

        public static ISocket CreateNonBlockingUdpBroadcastSocket()
        {
            ISocket socketWrapper = new NonBlockingUdpBroadcastImpl();
            return socketWrapper;
        }

        public static ISocket CreateNonBlockingUdpMulticastSocket(EndPoint multicastAddress, EndPoint localAddress, bool loopback, int ttl)
        {
            ISocket socketWrapper = new NonBlockingUdpMulticastImpl(multicastAddress, localAddress, loopback, ttl);
            return socketWrapper;
        }

        public static ISocket CreateNonBlockingUdpP2PSocket()
        {
            ISocket socketWrapper = new NonBlockingUdpP2PImpl();
            return socketWrapper;
        }
    }
}
