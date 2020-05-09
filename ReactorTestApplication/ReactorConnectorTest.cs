using AcceptorConnector.Interface;
using log4net;
using Reactor.Interface;
using Reactor.Class;
using Reactor.Utilities;
using ServiceLayer.Class;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace ReactorTestApplication
{

    public class ReactorConnectorTest : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReactorConnectorTest));

        private IReactor reactor;
        private IConnector connector;
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
        public ReactorConnectorTest(int reactorTimerMicroSeconds)
        {
            reactor = new ReactorSocketImpl();
            reactorTimeout = reactorTimerMicroSeconds;
            connectionList = new List<IServiceHandler>();
            connector = new Tcp_SocketConnector(reactor);
            ThreadStart threadStart = new ThreadStart(Run);
            reactorThread = new Thread(threadStart);
            running = true;
            reactorThread.Start();
        }

        public void Run()
        {
            while (running)
            {
                reactor.HandleEvents(reactorTimeout);
            }

            this.Dispose();
        }

        public void Send(string message)
        {
            foreach (var connection in connectionList)
            {
                connection.WriteData = System.Text.Encoding.UTF8.GetBytes(message);
            }
        }

        public void AddConnection(EndPoint remoteAddress, bool keepAlive, bool stream)
        {
            IServiceHandler serviceHandler;
            if (keepAlive && !stream)
            {
                serviceHandler = new TcpVarData_SockServiceHandler(reactor, true, 5000);
            }
            else if(!keepAlive && !stream)
            {
                serviceHandler = new TcpVarData_SockServiceHandler(reactor, false, 0);
            }
            else
            {
                serviceHandler = new TcpEoLData_SockServiceHandler(reactor);
            }
            
            ISocket socket = SocketFactory.CreateNonBlockingTcpSocket();
            serviceHandler.Handle = socket;
            serviceHandler.ConnectionNotification += ConnectNotification;
            connector.Connect(serviceHandler, remoteAddress, Connection_Mode.ASYNC);
        }

        public void Dispose()
        {
            foreach (var connection in connectionList)
            {
                RemoveEventHandlerNotifications(connection);
                connection.Dispose();
            }
            connectionList.Clear();
            reactor?.Dispose();
        }

        private void ConnectNotification(IServiceHandler serviceHandler, ConnectionResult result)
        {
            serviceHandler.ConnectionNotification -= ConnectNotification;
            if(result == ConnectionResult.SUCCESS)
            {
                SetupEventHandlerNotifications(serviceHandler);
                connectionList.Add(serviceHandler);
                socketEvent?.Invoke(SocketEventType.ConnectionEvent);
            }
            else
            {
                socketEvent?.Invoke(SocketEventType.ConnectionError);
            }            
        }

        private void ReadCompleteNotification(IServiceHandler serviceHandler)
        {
            var str = System.Text.Encoding.UTF8.GetString(serviceHandler.ReadData);
            log.InfoFormat("Message from {0} : {1} --- {2}", ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                                                                ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString(), str);
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
            log.InfoFormat("Connection to {0} : {1} closed and removed from connector list.",
                            ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                            ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString());
            RemoveEventHandlerNotifications(serviceHandler);
            connectionList.Remove(serviceHandler);
            socketEvent?.Invoke(SocketEventType.CloseEvent);
        }

        private void ErrorNotification(IServiceHandler serviceHandler, Exception ex)
        {
            log.InfoFormat("Connection to {0} : {1} closed due to error : {2}",
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
