using Reactor.Interface;
using System;
using System.Net;

namespace AcceptorConnector.Interface
{
    public enum ConnectionResult
    {
        SUCCESS = 0,
        FAIL
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IServiceHandler : IEventHandler
    {
        /// <summary>
        /// The remote endpoint for the service
        /// </summary>
        EndPoint RemoteURI { get; set; }
        byte[] ReadData { get; }
        byte[] WriteData { set; }
        Action<IServiceHandler> ReadCompleteNotification { get; set; }
        Action<IServiceHandler> WriteCompleteNotification { get; set; }
        Action<IServiceHandler> CloseEventNotification { get; set; }
        Action<IServiceHandler, ConnectionResult> ConnectionNotification { get; set; }
        Action<IServiceHandler,Exception> ErrorNotification { get; set; }
        /// <summary>
        /// Opens the service handler to send and receive data, registers it with the reactor
        /// </summary>
        void Open();
    }
}
