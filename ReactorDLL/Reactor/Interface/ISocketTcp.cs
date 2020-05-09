using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactor.Reactor.Interface
{
    interface ISocketTcp
    {
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
    }
}
