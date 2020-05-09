using System.Net;

namespace AcceptorConnector.Interface
{
    /// <summary>
    /// Enumeration for Asynchronous and Synchronous connection mode
    /// </summary>
    public enum Connection_Mode
    {
        SYNC = 0,
        ASYNC = 1
    }
    /// <summary>
    /// A Connector class handles the connection service connecting a Service Handler to a remote endpoint 
    /// synchronously or asynchronously 
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        /// Connects a service handler to a remote uri endpoint synchronously or asynchronously 
        /// </summary>
        /// <param name="serviceHandler">The service handler to connect</param>
        /// <param name="remoteEndPoint">The endpoint to connect to</param>
        /// <param name="mode">The connection mode, Synchronoud or Asynchromous</param>
        void Connect(IServiceHandler serviceHandler, EndPoint remoteEndPoint, Connection_Mode mode);
    }
}
