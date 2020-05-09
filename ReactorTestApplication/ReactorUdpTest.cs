using AcceptorConnector.Interface;
using ServiceLayer.Class;
using log4net;
using Reactor.Interface;
using Reactor.Class;
using System;
using System.Net;
using System.Threading;
using MsgPack;
using MsgPack.Serialization;
using System.Collections.Generic;

namespace ReactorTestApplication
{
    public class ReactorUdpTest : IDisposable
    {
        public enum UdpTestType
        {
            BROADCAST = 0,
            MULTICAST,
            P2P
        }


        private static readonly ILog log = LogManager.GetLogger(typeof(ReactorConnectorTest));

        private IReactor reactor;
        private Thread reactorThread;
        private int reactorTimeout;
        private volatile bool running = false;
        private Action<SocketEventType> socketEvent;
        private IServiceHandler serviceHandler;

        public bool Running { set { running = value; } }
        public Action<SocketEventType> SocketEvent
        {
            get { return socketEvent; }
            set { socketEvent = value; }
        }

        public ReactorUdpTest(UdpTestType testtype, EndPoint localEndPoint, EndPoint remoteEndPoint,int reactorTimerMicroSeconds)
        {
            reactor = new ReactorSocketImpl();
            this.reactorTimeout = reactorTimerMicroSeconds;
            Open(testtype, localEndPoint, remoteEndPoint);
            ThreadStart threadStart = new ThreadStart(Run);
            reactorThread = new Thread(threadStart);
            running = true;
            reactorThread.Start();
        }

        public void Send(string message)
        {
            serviceHandler.WriteData = System.Text.Encoding.UTF8.GetBytes(message);
        }

        public void Dispose()
        {
            RemoveEventHandlerNotifications(serviceHandler);
            serviceHandler.Dispose();
            reactor.Dispose();
        }

        private void Run()
        {
            while (running)
            {
                reactor.HandleEvents(reactorTimeout);
            }

            this.Dispose();
        }

        private void Open(UdpTestType testType, EndPoint bindEndPoint, EndPoint connectEndPoint)
        {
            switch (testType)
            {
                case UdpTestType.BROADCAST:
                    {
                        serviceHandler = new UdpBroadcast_ServiceHandler(this.reactor, bindEndPoint, connectEndPoint);
                        break;
                    }
                case UdpTestType.MULTICAST:
                    {
                        serviceHandler = new UdpMulticast_ServiceHandler(this.reactor, bindEndPoint, connectEndPoint, false, 4);
                        break;
                    }
                case UdpTestType.P2P:
                    {
                        serviceHandler = new UdpP2P_ServiceHandler(this.reactor, bindEndPoint, connectEndPoint);
                        break;
                    }
                default:
                    break;
            }

            SetupEventHandlerNotifications(serviceHandler);
            serviceHandler.Open();
        }

        private void ReadCompleteNotification(IServiceHandler serviceHandler)
        {

            //             byte[] dataType = new byte[4];
            //             byte[] data = new byte[serviceHandler.ReadData.Length - 4];
            //             Buffer.BlockCopy(serviceHandler.ReadData, 0, dataType, 0, 4);
            //             Buffer.BlockCopy(serviceHandler.ReadData, 4, data, 0, serviceHandler.ReadData.Length - 4);
            //             var integer_serializer = MessagePackSerializer.Get<int>(); // added msg pack deserialiser
            //             var deserializedDataType = integer_serializer.UnpackSingleObject(dataType);
            //             switch(deserializedDataType)
            //             {
            //                 case 0:
            //                     var clock_serializer = MessagePackSerializer.Get<double>();
            //                     break;
            //                 case 1:
            //                     var date_serializer = MessagePackSerializer.Get<Array<int, 6>>();
            //                     break;
            //                 default:
            //                     break;
            //             }

            var rawObject = Unpacking.UnpackObject(serviceHandler.ReadData);

            if(rawObject.Value.IsArray)
            {
                log.InfoFormat("Is Array ? {0}", rawObject.Value.IsArray);
                var arrayObject = rawObject.Value.AsList();
                List<int> dateList = new List<int>();
                foreach(var obj in arrayObject)
                {
                    dateList.Add(obj.AsInt32());
//                     log.InfoFormat("Container at {0} is of type {1}",
//                         arrayObject.IndexOf(obj),
//                         obj.UnderlyingType);
                }

                string listAsString = string.Join(", ", dateList.ToArray());
                log.InfoFormat("Array contains [{0}]", listAsString);
            }
            else if(rawObject.Value.UnderlyingType == typeof(double))
            {
                var double_serializer = MessagePackSerializer.Get<double>();
                var deserialized = double_serializer.UnpackSingleObject(serviceHandler.ReadData);
                log.InfoFormat("Message received on {0} : {1} --- {2}  : Type is {3}", 
                    ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                     ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString(), 
                     deserialized,
                     rawObject.Value.UnderlyingType);
            }
            

            //log.InfoFormat("Message received on {0} : {1} --- {2}", ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
            //                                                    ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString(), deserialized);
            //var str = System.Text.Encoding.UTF8.GetString(serviceHandler.ReadData);
//             log.InfoFormat("Message received on {0} : {1} --- {2}", ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
//                                                                 ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString(), str);
            socketEvent?.Invoke(SocketEventType.ReadCompleteEvent);
        }

        private void WriteCompleteNotification(IServiceHandler serviceHandler)
        {
            log.InfoFormat("Message sent to {0} : {1}", ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                                                        ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString());
            socketEvent?.Invoke(SocketEventType.WriteCompleteEvent);
        }

        private void ErrorNotification(IServiceHandler serviceHandler, Exception ex)
        {
            log.InfoFormat("Connection to {0} : {1} closed due to error: {2}",
                            ((IPEndPoint)serviceHandler.RemoteURI).Address.ToString(),
                            ((IPEndPoint)serviceHandler.RemoteURI).Port.ToString(),
                            ex.Message);
            RemoveEventHandlerNotifications(serviceHandler);
            socketEvent?.Invoke(SocketEventType.CloseEvent);
        }

        private void SetupEventHandlerNotifications(IServiceHandler serviceHandler)
        {
            serviceHandler.ReadCompleteNotification += ReadCompleteNotification;
            serviceHandler.WriteCompleteNotification += WriteCompleteNotification;
            //serviceHandler.CloseEventNotification += CloseEventNotification;
            serviceHandler.ErrorNotification += ErrorNotification;
        }

        private void RemoveEventHandlerNotifications(IServiceHandler serviceHandler)
        {
            serviceHandler.ReadCompleteNotification -= ReadCompleteNotification;
            serviceHandler.WriteCompleteNotification -= WriteCompleteNotification;
            //serviceHandler.CloseEventNotification -= CloseEventNotification;
            serviceHandler.ErrorNotification -= ErrorNotification;
        }
    }
}
