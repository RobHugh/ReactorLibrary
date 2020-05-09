using System;
using System.Collections;
using System.Collections.Concurrent;
using Reactor.Interface;
using slf4net;

namespace Reactor.Class
{
    internal class DemuxTable 
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(DemuxTable));

        private ConcurrentDictionary<IntPtr, Tuple<ISocket, IEventHandler, EventType> > table = 
            new ConcurrentDictionary<IntPtr, Tuple<ISocket, IEventHandler, EventType> >();

        ConcurrentDictionary<IntPtr, Tuple<ISocket, IEventHandler, EventType>> Table { get { return table; } }


        internal void ConvertToSelectLists(IList readList, IList writeList, IList exceptList)
        {
            foreach (var pair in table)
            {
                if((pair.Value.Item3 & (EventType.READ_EVENT | EventType.ACCEPT_EVENT)) == (EventType.READ_EVENT | EventType.ACCEPT_EVENT))
                {
                    readList.Add(pair.Value.Item1.UserSocket);
                    log.Debug(String.Format("{0} Socket with handle {1} entered into SELECT list as READ/ACCEPT select",
                                pair.Value.Item2.GetType().ToString(), pair.Value.Item1.Handle.ToString()));
                }
                if (((pair.Value.Item3 & EventType.WRITE_EVENT) == EventType.WRITE_EVENT) ||
                       ((pair.Value.Item3 & EventType.CONNECT_EVENT) == EventType.CONNECT_EVENT))
                {
                    writeList.Add(pair.Value.Item1.UserSocket);
                    log.Debug(String.Format("{0} Socket with handle {1} entered into SELECT list as WRITE select",
                                pair.Value.Item2.GetType().ToString(), pair.Value.Item1.Handle.ToString()));
                }

                if ((pair.Value.Item3 & EventType.CONNECT_EVENT) == EventType.CONNECT_EVENT)
                {
                    writeList.Add(pair.Value.Item1.UserSocket);
                    log.Debug(String.Format("{0} Socket with handle {1} entered into SELECT list as CONNECT select",
                                pair.Value.Item2.GetType().ToString(), pair.Value.Item1.Handle.ToString()));

                    exceptList.Add(pair.Value.Item1.UserSocket);
                    log.Debug(String.Format("{0} Socket with handle {1} entered into SELECT list as EXCEPT select",
                                    pair.Value.Item2.GetType().ToString(), pair.Value.Item1.Handle.ToString()));
                }


            }
        }

        internal void HandleSelectEvents(IList readList, IList writeList, IList exceptList)
        {
            foreach (var entry in table)
            {
                if (readList.Contains(entry.Value.Item1.UserSocket))
                {
                    // socket is either ready for reading or ready for accepting
                    if (entry.Value.Item3 == EventType.ACCEPT_EVENT)
                    {
                        entry.Value.Item2.HandleEvent(entry.Value.Item1, EventType.ACCEPT_EVENT);
                    }
                    else
                    {
                        entry.Value.Item2.HandleEvent(entry.Value.Item1, EventType.READ_EVENT);
                    }                               
                }
                if (writeList.Contains(entry.Value.Item1.UserSocket))
                {
                    // socket is ready for writing
                    if(entry.Value.Item3 == EventType.CONNECT_EVENT)
                    {
                        entry.Value.Item2.HandleEvent(entry.Value.Item1, EventType.CONNECT_EVENT);
                    }
                    else
                    {
                        entry.Value.Item2.HandleEvent(entry.Value.Item1, EventType.WRITE_EVENT);
                    }
                    
                }
                if (exceptList.Contains(entry.Value.Item1.UserSocket))
                {
                    // socket has an exception (non blocking connect call that hasn't connected)
                    entry.Value.Item2.HandleEvent(entry.Value.Item1, EventType.ERROR_EVENT);
                }
            }
        }

        internal void AddToTable(Tuple<ISocket, IEventHandler, EventType> entry)
        {
            try
            {
                if (table.TryAdd(entry.Item1.Handle, entry))
                {
                    log.Info(String.Format("{0} entered into reactor demux table as event type {1}",
                                entry.Item2.GetType().ToString(), entry.Item3.ToString()));
                }
                else
                {
                    log.Warn(String.Format("{0} FAILED entry into reactor demux table as event type {1}",
                                entry.Item2.GetType().ToString(), entry.Item3.ToString()));
                }
            }
            catch(ArgumentNullException ane)
            {
                try
                {
                	log.Warn(ane, String.Format("Socket handle for {0} is null", entry.Item2.GetType().ToString()));
                }
                catch (NullReferenceException nrex)
                {
                    log.Error(nrex, "Attempt to add a NULL event handler to the reactor demux table");
                }
            }
            catch(OverflowException oe)
            {
                log.Error(oe, "Reactor Demux table already contains maximum number of elements");
            }
        }

        internal void RemoveFromTable(Tuple<ISocket, IEventHandler, EventType> entry)
        {
            try
            {
                Tuple<ISocket, IEventHandler, EventType> removedEntry;
                if (table.TryRemove(entry.Item1.Handle, out removedEntry))
                {
                    log.Debug(String.Format("{0} successfully removed from reactor Demux table", 
                                removedEntry.Item2.GetType().ToString()));
                }
                else
                {
                    log.Warn(String.Format("{0} FAILED to be removed from reactor Demux table",
                                entry.Item2.GetType().ToString()));
                }
            }
            catch(ArgumentNullException ane)
            {
                log.Warn(ane, "SocketHandle for {0} is Null", entry.Item2.GetType().ToString());
            }
        }

        public void Dispose()
        {
            foreach (Tuple<ISocket, IEventHandler, EventType> entry in table.Values)
            {
                entry.Item2.Dispose();
            }
            table.Clear();
            log.Info("Reactor Demux Table is Disposed and all entries removed and disposed");
        }
    }
}
