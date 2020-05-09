using AcceptorConnector.Interface;
using Reactor.Interface;
using Reactor.Class;
using ServiceLayer.Class;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System;
using log4net;

namespace ReactorTestApplication
{
    class ReactorAcceptorTest : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReactorAcceptorTest));

        private IReactor reactor;
        private IAcceptor acceptor;
        private List<IServiceHandler> connectionList;
        private Thread reactorThread;
        private int reactorTimeout;
        private volatile bool running = false;
        private Action<SocketEventType> socketEvent;

        public bool Running { set { running = value; } }
        public Action<SocketEventType> SocketEvent
        {
            get { return socketEvent; }
            set { socketEvent = value; }
        }

        public ReactorAcceptorTest(IPAddress acceptorAddress, int acceptorPort, 
                                    int reactorTimerMicroSeconds, int pendingConnections, bool keepAlive, bool isStream)
        {
            reactor = new ReactorSocketImpl();
            reactorTimeout = reactorTimerMicroSeconds;
            connectionList = new List<IServiceHandler>();
            IPEndPoint endpoint = new IPEndPoint(acceptorAddress, acceptorPort);

            if(keepAlive && !isStream)
            {
                acceptor = new Tcp_SocketAcceptor(endpoint, reactor, pendingConnections, true, 5000, false);
            }
            else if(!keepAlive && !isStream)
            {
                acceptor = new Tcp_SocketAcceptor(endpoint, reactor, pendingConnections, false, 0, false);
            }
            else if(isStream)
            {
                acceptor = new Tcp_SocketAcceptor(endpoint, reactor, pendingConnections, false, 0, true);
            }
            
            acceptor.AcceptNotification += AcceptNotification;
            ThreadStart threadStart = new ThreadStart(Run);
            reactorThread = new Thread(threadStart);
            running = true;
            reactorThread.Start();
        }

        public void Send(string message)
        {
            foreach (var connection in connectionList)
            {
                connection.WriteData = System.Text.Encoding.UTF8.GetBytes(message);
            }
        }

        public void Run()
        {
            while(running)
            {
                reactor.HandleEvents(reactorTimeout);
            }

            this.Dispose();
        }

        public void Dispose()
        {
            foreach(var connection in connectionList)
            {
                RemoveEventHandlerNotifications(connection);
                connection.Dispose();
            }
            connectionList.Clear();
            acceptor.AcceptNotification -= AcceptNotification;
            acceptor?.Dispose();
            reactor?.Dispose();
        }

        private void AcceptNotification()
        {
            IServiceHandler connection = acceptor.ServiceHandler;
            SetupEventHandlerNotifications(connection);
            connectionList.Add(connection);
            log.InfoFormat("Connection to {0} : {1} added to acceptor list.",
                            ((IPEndPoint)connection.RemoteURI).Address.ToString(),
                            ((IPEndPoint)connection.RemoteURI).Port.ToString());
            socketEvent?.Invoke(SocketEventType.AcceptEvent);
        }

        private void ReadCompleteNotification(IServiceHandler serviceHandler)
        {
            var str = System.Text.Encoding.UTF8.GetString(serviceHandler.ReadData);
            log.InfoFormat("Message from {0} : {1} --- {2}", ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                                                                ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString(),str);
            socketEvent?.Invoke(SocketEventType.ReadCompleteEvent);
        }

        private void WriteCompleteNotification(IServiceHandler serviceHandler)
        {
            log.InfoFormat("Message sent to {0} : {1}", ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                                                        ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString());
            socketEvent?.Invoke(SocketEventType.WriteCompleteEvent);
        }

        private void CloseEventNotification(IServiceHandler serviceHandler)
        {
            log.InfoFormat("Connection to {0} : {1} closed and removed from acceptor list.",
                            ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                            ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString());
            RemoveEventHandlerNotifications(serviceHandler);
            connectionList.Remove(serviceHandler);
            socketEvent?.Invoke(SocketEventType.CloseEvent);
        }

        private void ErrorNotification(IServiceHandler serviceHandler, Exception ex)
        {
            log.InfoFormat("Connection to {0} : {1} closed due to error: {2}",
                            ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                            ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString(),
                            ex.Message);
            RemoveEventHandlerNotifications(serviceHandler);
            connectionList.Remove(serviceHandler);
            socketEvent?.Invoke(SocketEventType.CloseEvent);
        }

        private void SetupEventHandlerNotifications(IServiceHandler serviceHandler)
        {
            serviceHandler.ReadCompleteNotification += ReadCompleteNotification;
            serviceHandler.WriteCompleteNotification += WriteCompleteNotification;
            serviceHandler.CloseEventNotification += CloseEventNotification;
            serviceHandler.ErrorNotification += ErrorNotification;
        }

        private void RemoveEventHandlerNotifications(IServiceHandler serviceHandler)
        {
            serviceHandler.ReadCompleteNotification -= ReadCompleteNotification;
            serviceHandler.WriteCompleteNotification -= WriteCompleteNotification;
            serviceHandler.CloseEventNotification -= CloseEventNotification;
            serviceHandler.ErrorNotification -= ErrorNotification;
        }
    }
}
