using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using System.Net;

namespace ReactorTestApplication
{
    public enum SocketEventType
    {
        ConnectionError = 0,
        ConnectionEvent,
        AcceptEvent,
        ReadCompleteEvent,
        WriteCompleteEvent,
        CloseEvent
    }

    delegate void EnableDelegate(bool enable);

    public partial class FormTestApplication : Form
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(FormTestApplication));

        private OrderedDictionary localAddresses;
        private ReactorAcceptorTest acceptorTest;
        private ReactorConnectorTest connectorTest;
        private ReactorUdpTest udpTest;
        IPAddress[] addresses;
        string[] names;

        public FormTestApplication()
        {
            InitializeComponent();
            localAddresses = new OrderedDictionary();
        }

        private void FormTestApplication_Load(object sender, EventArgs e)
        {
            XmlConfigurator.Configure();
            ListViewAppender.ConfigureListViewAppender(listViewDataOutput);
            
            Dictionary<IPAddress, string> networkInterfaces = Reactor.Utilities.NetworkInterfaces.GetIPv4NetworkInterfaces();
            foreach(var entry in networkInterfaces)
            {
                localAddresses.Add(entry.Key, entry.Value);
            }

            addresses = new IPAddress[localAddresses.Count];
            names = new string[localAddresses.Count];
            localAddresses.Keys.CopyTo(addresses, 0);
            localAddresses.Values.CopyTo(names, 0);

            for(int i = 0; i < localAddresses.Count; ++i)
            {          
                string entry = string.Concat(addresses[i].ToString(), " (", names[i], ")");
                comboBoxLocalAddresses.Items.Add(entry);
            }

            buttonStart.Enabled = false;
            buttonStop.Enabled = false;
            panelAddresses.Enabled = false;
            panelSendMessage.Enabled = false;
            comboBoxLocalAddresses.SelectedIndex = 0;

            DateTime now = DateTime.Now;
            log.InfoFormat("Application Started {0}", now.ToShortTimeString());
        }

        public void OnStart_FormChanges()
        {
            EnableStartButton(false);
            EnableStopButton(true);
            EnableAddressPanel(false);
        }

        public void OnStop_FormChanges()
        {
            EnableStartButton(true);
            EnableStopButton(false);
            EnableAddressPanel(true);
            EnableMessageBox(false);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            switch (comboBoxTestType.SelectedIndex)
            {
                case 0:
                    {
                        acceptorTest = new ReactorAcceptorTest(addresses[comboBoxLocalAddresses.SelectedIndex], 
                                                                Int32.Parse(textBoxLocalPort.Text),
                                                                500000, 2, false, false);
                        acceptorTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        break;
                    }
                case 1:
                    {
                        connectorTest = new ReactorConnectorTest(500000);
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(textBoxRemoteAddress.Text), 
                                                             Int32.Parse(textBoxRemotePort.Text));
                        connectorTest.AddConnection(endPoint, false, false);
                        connectorTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        break;
                    }
                case 2:
                    {
                        acceptorTest = new ReactorAcceptorTest(addresses[comboBoxLocalAddresses.SelectedIndex],
                                                                Int32.Parse(textBoxLocalPort.Text),
                                                                500000, 2, true, false);
                        acceptorTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        break;
                    }
                case 3:
                    {
                        connectorTest = new ReactorConnectorTest(500000);
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(textBoxRemoteAddress.Text),
                                                             Int32.Parse(textBoxRemotePort.Text));
                        connectorTest.AddConnection(endPoint, true, false);
                        connectorTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        break;
                    }
                case 4:
                    {
                        IPEndPoint localEndPoint = new IPEndPoint(addresses[comboBoxLocalAddresses.SelectedIndex], 
                                                                Int32.Parse(textBoxLocalPort.Text));
                        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(textBoxRemoteAddress.Text),
                                                                Int32.Parse(textBoxRemotePort.Text));
                        udpTest = new ReactorUdpTest(ReactorUdpTest.UdpTestType.BROADCAST, localEndPoint, remoteEndPoint, 5000000);
                        udpTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        EnableMessageBox(true);
                        break;
                    }
                case 5:
                    {
                        IPEndPoint localEndPoint = new IPEndPoint(addresses[comboBoxLocalAddresses.SelectedIndex],
                                                                Int32.Parse(textBoxLocalPort.Text));
                        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(textBoxRemoteAddress.Text),
                                                                Int32.Parse(textBoxRemotePort.Text));
                        udpTest = new ReactorUdpTest(ReactorUdpTest.UdpTestType.MULTICAST, localEndPoint, remoteEndPoint, 5000000);
                        udpTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        EnableMessageBox(true);
                        break;
                    }
                case 6:
                    {
                        IPEndPoint localEndPoint = new IPEndPoint(addresses[comboBoxLocalAddresses.SelectedIndex],
                                                                Int32.Parse(textBoxLocalPort.Text));
                        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(textBoxRemoteAddress.Text),
                                                                Int32.Parse(textBoxRemotePort.Text));
                        udpTest = new ReactorUdpTest(ReactorUdpTest.UdpTestType.P2P, localEndPoint, remoteEndPoint, 5000000);
                        udpTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        EnableMessageBox(true);
                        break;
                    }
                case 7:
                    {
                        acceptorTest = new ReactorAcceptorTest(addresses[comboBoxLocalAddresses.SelectedIndex],
                                        Int32.Parse(textBoxLocalPort.Text),
                                        500000, 2, false, true);
                        acceptorTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        break;
                    }
                case 8:
                    {
                        connectorTest = new ReactorConnectorTest(500000);
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(textBoxRemoteAddress.Text),
                                                             Int32.Parse(textBoxRemotePort.Text));
                        connectorTest.AddConnection(endPoint, false, true);
                        connectorTest.SocketEvent += OnSocketEvent;
                        OnStart_FormChanges();
                        break;
                    }
                default:
                    break;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            switch (comboBoxTestType.SelectedIndex)
            {
                case 0:
                    {
                        acceptorTest.Running = false;
                        acceptorTest.SocketEvent -= OnSocketEvent;
                        acceptorTest = null;
                        OnStop_FormChanges();                      
                        break;
                    }
                case 1:
                    {
                        connectorTest.Running = false;
                        connectorTest.SocketEvent -= OnSocketEvent;
                        connectorTest = null;
                        OnStop_FormChanges();
                        break;
                    }
                case 2:
                    {
                        acceptorTest.Running = false;
                        acceptorTest.SocketEvent -= OnSocketEvent;
                        acceptorTest = null;
                        OnStop_FormChanges();
                        break;
                    }
                case 3:
                    {
                        connectorTest.Running = false;
                        connectorTest.SocketEvent -= OnSocketEvent;
                        connectorTest = null;
                        OnStop_FormChanges();
                        break;
                    }
                case 4:
                    {
                        udpTest.Running = false;
                        udpTest.SocketEvent -= OnSocketEvent;
                        udpTest = null;
                        OnStop_FormChanges();
                        break;
                    }
                case 5:
                    {
                        udpTest.Running = false;
                        udpTest.SocketEvent -= OnSocketEvent;
                        udpTest = null;
                        OnStop_FormChanges();
                        break;
                    }
                case 6:
                    {
                        udpTest.Running = false;
                        udpTest.SocketEvent -= OnSocketEvent;
                        udpTest = null;
                        OnStop_FormChanges();
                        break;
                    }
                case 7:
                    {
                        acceptorTest.Running = false;
                        acceptorTest.SocketEvent -= OnSocketEvent;
                        acceptorTest = null;
                        OnStop_FormChanges();
                        break;
                    }
                case 8:
                    {
                        connectorTest.Running = false;
                        connectorTest.SocketEvent -= OnSocketEvent;
                        connectorTest = null;
                        OnStop_FormChanges();
                        break;
                    }
                default:
                    break;
            }
        }

        private void comboBoxTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBoxTestType.SelectedIndex)
            {
                default:
                    {
                        OnStop_FormChanges();
                        break;
                    }
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            EnableMessageBox(false);
            if(connectorTest != null)
            {
                connectorTest.Send(textBoxSendMessage.Text);
            }
            else if(acceptorTest != null)
            {
                acceptorTest.Send(textBoxSendMessage.Text);
            }
            else if(udpTest != null)
            {
                udpTest.Send(textBoxSendMessage.Text);
            }
        }

        public void OnSocketEvent(SocketEventType eventType)
        {
            switch (eventType)
            {
                case SocketEventType.ConnectionError:
                    {
                        if(connectorTest != null)
                        {
                            connectorTest.SocketEvent -= OnSocketEvent;
                        }
                        
                        OnStop_FormChanges();
                        break;
                    }
                case SocketEventType.ConnectionEvent:
                    {
                        EnableMessageBox(true);
                        break;
                    }
                case SocketEventType.AcceptEvent:
                    {
                        EnableMessageBox(true);
                        break;
                    }
                case SocketEventType.ReadCompleteEvent:
                    {
                        break;
                    }
                case SocketEventType.WriteCompleteEvent:
                    {
                        EnableMessageBox(true);
                        break;
                    }
                case SocketEventType.CloseEvent:
                    {
                        if (connectorTest != null)
                        {
                            connectorTest.SocketEvent -= OnSocketEvent;
                            OnStop_FormChanges();
                        }
                        else if(acceptorTest != null)
                        {
                            EnableMessageBox(false);
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private void EnableStartButton(bool enable)
        {
            if(this.buttonStart.InvokeRequired)
            {
                var d = new EnableDelegate(EnableStartButton);
                this.Invoke(d, new object[] { enable });
            }
            else
            {
                this.buttonStart.Enabled = enable;
            }
        }

        private void EnableStopButton(bool enable)
        {
            if (this.buttonStop.InvokeRequired)
            {
                var d = new EnableDelegate(EnableStopButton);
                this.Invoke(d, new object[] { enable });
            }
            else
            {
                this.buttonStop.Enabled = enable;
            }
        }

        private void EnableAddressPanel(bool enable)
        {
            if (this.panelAddresses.InvokeRequired)
            {
                var d = new EnableDelegate(EnableAddressPanel);
                this.Invoke(d, new object[] { enable });
            }
            else
            {
                this.panelAddresses.Enabled = enable;
            }
        }

        private void EnableMessageBox(bool enable)
        {
            if (this.panelSendMessage.InvokeRequired)
            {
                var d = new EnableDelegate(EnableMessageBox);
                this.Invoke(d, new object[] { enable });
            }
            else
            {
                this.panelSendMessage.Enabled = enable;
            }
        }

        private void FormTestApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(null != connectorTest)
                connectorTest.Running = false;
            if(null != acceptorTest)
                acceptorTest.Running = false;
            if (null != udpTest)
                udpTest.Running = false;
        }
    }
}
