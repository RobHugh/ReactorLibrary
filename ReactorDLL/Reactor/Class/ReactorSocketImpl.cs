using Reactor.Interface;
using slf4net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Reactor.Class
{
    public class ReactorSocketImpl : IReactor
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(ReactorSocketImpl));

        private DemuxTable demuxTable;

        public ReactorSocketImpl()
        {
            demuxTable = new DemuxTable();
        }

        public void Dispose()
        {
            demuxTable.Dispose();
        }

        public void HandleEvents(int timeout)
        {
            List<Socket> readList = new List<Socket>();
            List<Socket> writeList = new List<Socket>();
            List<Socket> exceptList = new List<Socket>();

            demuxTable.ConvertToSelectLists(readList, writeList, exceptList);

            try
            {
                if((readList.Count > 0) || (writeList.Count > 0) || (exceptList.Count > 0))
                {
                    Socket.Select(readList, writeList, exceptList, timeout);
                }                
            }
            catch(ArgumentNullException)
            {
                // thrown if all of the lists are empty or null
                log.Debug("All select lists are empty or null");
                return;
            }
            catch(SocketException se)
            {
                // socket error cases
                log.Warn(se, "Socket Error");
                return;
            }

            demuxTable.HandleSelectEvents(readList, writeList, exceptList);
        }

        public void RegisterHandler(IEventHandler eh, EventType et)
        {
            Tuple<ISocket, IEventHandler, EventType> entry = Tuple.Create(eh.Handle, eh, et);
            demuxTable.AddToTable(entry);
        }

        public void RemoveHandler(IEventHandler eh, EventType et)
        {
            Tuple<ISocket, IEventHandler, EventType> entry = Tuple.Create(eh.Handle, eh, et);
            demuxTable.RemoveFromTable(entry); 
        }
    }
}
