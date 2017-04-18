using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS339_V2
{
    class Vlan : Interface
    {
        public Vlan(string chunk)
            : base(chunk)
        {
            String[] lines = chunk.Trim().Split('\n');
            findName(lines);
            findIPAddress(lines);

            if (ip != null)
            {
                findPrefix();
            }
        }
    }
}
