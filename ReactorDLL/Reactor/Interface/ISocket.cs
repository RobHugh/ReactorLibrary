using System;
using System.Net;
using System.Net.Sockets;

namespace Reactor.Interface
{
    /// <summary>
    /// An interface which wraps the functionality of .NET Socket class
    /// </summary>
    public interface ISocket : IDisposable
    {
        /// <summary>
        /// The Address handle of the Socket. Used for key indexing in demultiplexing table
        /// </summary>
        IntPtr Handle { get; }
        /// <summary>
        /// The remote address of the socket
        /// </summary>
        EndPoint RemoteEndPoint { get; }
        /// <summary>
        /// Handle to the underlying socket
        /// </summary>
        Socket UserSocket { get; }
        /// <summary>
        /// Associates a socket with a local endpoint, 
        /// see https://msdn.microsoft.com/en-us/library/system.net.sockets.socket.bind(v=vs.110).aspx
        /// </summary>
        /// <param name="ipep">The endpoint to associate with the socket</param>
        void Bind(EndPoint ipep);
        /// <summary>
        /// Places a socket in a listening state, 
        /// see https://msdn.microsoft.com/en-us/library/system.net.sockets.socket.listen(v=vs.110).aspx
        /// </summary>
        /// <param name="pendingConnections">The maximum length of the number of pending connections on the listen socket queue</param>
        void Listen(int pendingConnections);
        /// <summary>
        /// Creates a new socket for a newly created connection, 
        /// see https://msdn.microsoft.com/en-us/library/system.net.sockets.socket.accept(v=vs.110).aspx
        /// </summary>
        /// <returns>An interface which allows access to the newly created socket</returns>
        ISocket Accept();
        /// <summary>
        /// Establishes a connection to a remote host, 
        /// see https://msdn.microsoft.com/en-us/library/ych8bz3x(v=vs.110).aspx
        /// </summary>
        /// <param name="remoteEP">the endpoint that represents the remote device</param>
        void Connect(EndPoint remoteEP);
        /// <summary>
        /// Sends the specified number of bytes of data to a connected socket, starting at the specified offset, using the specified
        /// socket flags, see https://msdn.microsoft.com/en-us/library/4t14718h(v=vs.110).aspx
        /// </summary>
        /// <param name="buffer">Array of type byte that contains the data to send</param>
        /// <param name="offset">The position in the data buffer at which to begine sending data</param>
        /// <param name="length">The number of bytes to send</param>
        /// <param name="flags">Bitwise combination of SocketFlags values</param>
        /// <returns>The number of bytes sent to the socket</returns>
        int Send(byte[] buffer, int offset, int length, SocketFlags flags);
        /// <summary>
        /// Receives the specified number of bytes from a bound socket into the specified offset position of the receive buffer,
        /// using the specified SocketFlags, see https://msdn.microsoft.com/en-us/library/w3xtz6a5(v=vs.110).aspx
        /// </summary>
        /// <param name="buffer">An array  of type byte that is the storage location for the received data</param>
        /// <param name="offset">The location in buffer to store the received data</param>
        /// <param name="length">The number of bytes to receive</param>
        /// <param name="flags">Bitwise combination of SocketFlags values</param>
        /// <returns>The number of bytes received</returns>
        int Receive(byte[] buffer, int offset, int length, SocketFlags flags);
        /// <summary>
        /// Closes the socket and releases all underlying resources
        /// </summary>
        void Close();
    }
}
