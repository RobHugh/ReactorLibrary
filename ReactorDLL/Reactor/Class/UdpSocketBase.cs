using Reactor.Interface;
using System;
using System.Net;
using System.Net.Sockets;

namespace Reactor.Class
{
    public abstract class UdpSocketBase : ISocket
    {
        private Socket userSocket;
        private EndPoint remoteReceiveEndpoint;
        protected EndPoint bindEndpoint;
        protected EndPoint connectEndpoint;
        

        public IntPtr Handle
        {
            get
            {
                return userSocket.Handle;
            }
        }

        public EndPoint RemoteEndPoint
        {
            get
            {
                return connectEndpoint;
            }
        }

        public Socket UserSocket
        {
            get
            {
                return userSocket;
            }
        }

        public UdpSocketBase()
        {
            userSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            remoteReceiveEndpoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public virtual void Bind(EndPoint ipep)
        {
            bindEndpoint = ipep;
            userSocket.Bind(ipep);
        }

        public void Close()
        {
            userSocket.Close();
        }

        public virtual void Connect(EndPoint remoteEP)
        {
            connectEndpoint = remoteEP;
        }

        public virtual void Dispose()
        {
            userSocket.Close();
            userSocket.Dispose();
        }

        public int Receive(byte[] buffer, int offset, int length, SocketFlags flags)
        {
            return userSocket.ReceiveFrom(buffer, offset, length, flags, ref remoteReceiveEndpoint);
        }

        public int Send(byte[] buffer, int offset, int length, SocketFlags flags)
        {
            return userSocket.SendTo(buffer, offset, length, flags, connectEndpoint);
        }

        public void Listen(int pendingConnections)
        {
            throw new NotImplementedException("Udp sockets bind(), they do not listen().");
        }

        public ISocket Accept()
        {
            throw new NotImplementedException("Udp sockets do not implement Accept().");
        }
    }
}
