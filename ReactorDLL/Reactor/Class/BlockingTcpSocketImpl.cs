using Reactor.Interface;
using Reactor.Utilities;
using System;
using System.Net;
using System.Net.Sockets;

namespace Reactor.Class
{
    class BlockingTcpSocketImpl : ISocket
    {
        private Socket userSocket;
        Socket ISocket.UserSocket { get { return userSocket; } }
        IntPtr ISocket.Handle { get { return userSocket.Handle; } }
        public EndPoint RemoteEndPoint { get { return userSocket.RemoteEndPoint; } }

        public BlockingTcpSocketImpl()
        {
            userSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            userSocket.Blocking = true;
        }

        public BlockingTcpSocketImpl(Socket socket)
        {
            userSocket = socket;
            userSocket.Blocking = true;
        }
       
        public ISocket Accept()
        {
            ISocket ret = SocketFactory.CreateBlockingFromSocket(new BlockingTcpSocketImpl(userSocket.Accept()));
            return ret;
        }

        public void Bind(EndPoint ipep)
        {
            userSocket.Bind(ipep);
        }

        public void Close()
        {
            userSocket.Close();
        }

        public void Connect(EndPoint remoteEP)
        {
            userSocket.Connect(remoteEP);
        }

        public void Dispose()
        {
            userSocket.Close();
            userSocket.Dispose();
        }

        public void Listen(int pendingConnections)
        {
            userSocket.Listen(pendingConnections);
        }

        public int Receive(byte[] buffer, int offset, int length, SocketFlags flags)
        {
            return userSocket.Receive(buffer, offset, length, flags);
        }

        public int Send(byte[] buffer, int offset, int length, SocketFlags flags)
        {
            return userSocket.Send(buffer, offset, length, flags);
        }
    }
}
