using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS339_V2
{
    class Vlan : Interface
    {
        public string bridgeIP;
        public void populate(string starterIP, string starterMask, string bridgeIP)
        {
            prefix = findPrefix(starterIP, starterMask);
            this.bridgeIP = bridgeIP;
        }
    }
}