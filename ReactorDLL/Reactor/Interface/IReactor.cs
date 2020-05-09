using System;

namespace Reactor.Interface
{
    public enum EventType
    {
        ACCEPT_EVENT = 0x01,
        READ_EVENT = 0x01,        
        WRITE_EVENT = 0x02,
        READ_WRITE_EVENT = 0x03,
        CONNECT_EVENT = 0x04,
        ERROR_EVENT = 0x08,
        TIMEOUT_EVENT = 0x10,
        CLOSE_EVENT = 0x20,
        WRITE_COMPLETE = 0x40,
        READ_COMPLETE = 0x80
    }

    public interface IReactor : IDisposable
    {
        /// <summary>
        /// Registers an event handler implementation of a socket event to be entered into the reactor demultiplexing table
        /// </summary>
        /// <param name="eh">Concrete implementation of event handler to be registered</param>
        /// <param name="et">The event type of the event handler</param>
        /// <exception cref="NullReferenceException"> Throws null reference exception should eh param be null or should its underlying socket impl be null</exception>
        void RegisterHandler(IEventHandler eh, EventType et);
        /// <summary>
        /// Removes an event handler implementation of a socket event that has been entered into the reactor demultiplexing table
        /// </summary>
        /// <param name="eh">Concrete implementation of event handler to be unregistered</param>
        /// <param name="et">The event type of the event handler</param>
        /// <exception cref="NullReferenceException"> Throws null reference exception should eh param be null or should its underlying socket impl be null</exception>
        void RemoveHandler(IEventHandler eh, EventType et);
        /// <summary>
        /// Uses the demultiplexor to select sockets signaling required action and passes these to the demultiplexing table to handle the selected events
        /// </summary>
        /// <param name="timeout">The number of microseconds that the demultiplexor will wait for activity on the sockets before continuing program execution</param>
        void HandleEvents(int timeout);        
    }
}
