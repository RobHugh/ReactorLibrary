using System;

namespace Reactor.Interface
{
    /// <summary>
    /// An Event Handler that will be called from the Reactor when there is activity on the associated IOHandle
    /// </summary>
    public interface IEventHandler  : IDisposable 
    {
        /// <summary>
        /// The IO Handle associated with the Event Handler
        /// </summary>
        ISocket Handle { get; set; }
        /// <summary>
        /// Method to handle event activity that is called by the Reactor
        /// </summary>
        /// <param name="handle">The IOHandler to handle an event on</param>
        /// <param name="eventType">The Event Type to handle</param>
        void HandleEvent(ISocket handle, EventType eventType);
    }
}
