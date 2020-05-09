using Reactor.Interface;
using Reactor.Utilities;
using System;
using System.Net;
using System.Net.Sockets;

namespace Reactor.Class
{
    internal class NonBlockingTcpSocketImpl : ISocket
    {
        private Socket userSocket;
        Socket ISocket.UserSocket { get { return userSocket; } }
        IntPtr ISocket.Handle { get { return userSocket.Handle; } }
        public EndPoint RemoteEndPoint { get { return userSocket.RemoteEndPoint; } }

        public NonBlockingTcpSocketImpl()
        {
            userSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            userSocket.Blocking = false;
        }
        public NonBlockingTcpSocketImpl(Socket socket)
        {
            userSocket = socket;
            userSocket.Blocking = false;
        }

        public virtual void Bind(EndPoint ipep)
        {
            userSocket.Bind(ipep);
        }

        public virtual void Listen(int pendingConnections)
        {
            userSocket.Listen(pendingConnections);
        }

        public virtual ISocket Accept()
        {
            ISocket ret = SocketFactory.CreateNonBlockingFromSocket(new NonBlockingTcpSocketImpl(userSocket.Accept()));
            return ret;
        }

        public virtual void Connect(EndPoint remoteEP)
        {
            userSocket.Connect(remoteEP);
        }

        public virtual int Send(byte[] buffer, int offset, int length, SocketFlags flags)
        {
            return userSocket.Send(buffer, offset, length, flags);
        }

        public virtual int Receive(byte[] buffer, int offset, int length, SocketFlags flags)
        {
            return userSocket.Receive(buffer, offset, length, flags);
        }

        public virtual void Close()
        {
            userSocket.Close();
        }

        public virtual void Dispose()
        {
            userSocket.Close();
            userSocket.Dispose();
        }
    }
}
