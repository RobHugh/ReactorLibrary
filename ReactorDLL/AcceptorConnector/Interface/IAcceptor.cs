using Reactor.Interface;
using System;


namespace AcceptorConnector.Interface
{
    /// <summary>
    /// An Acceptor service that accepts incoming IO connections
    /// </summary>
    /// <typeparam name="TIOHandle">IOHandle Type</typeparam>
    public interface IAcceptor : IEventHandler
    {
        IServiceHandler ServiceHandler { get; set; }
        Action AcceptNotification { get; set; }
        /// <summary>
        /// Creates a concrete implementation of an acceptor service and activates it.
        /// </summary>
        void Accept();
    }
}
