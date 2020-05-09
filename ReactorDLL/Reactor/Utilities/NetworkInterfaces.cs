using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Reactor.Utilities
{
    public static class NetworkInterfaces
    {
        public static Dictionary<IPAddress, string> GetIPv4NetworkInterfaces()
        {
            Dictionary<IPAddress, string> interfaceList = new Dictionary<IPAddress, string>();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            interfaceList.Add(ip.Address, nic.Name);
                        }
                    }
                }            
            }
            return interfaceList;
        }
    }
}
