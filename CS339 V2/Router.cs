using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS339_V2
{
    class Router
    {
        public string name;
        public List<Interface> interfaces;
        public List<Vlan> vlans;

        public Router(string fileName, string contents)
        {
            this.name = fileName;
            String[] chunks = contents.Split('!');
            findInterfaces(chunks);
            findIPRoutes(chunks);
        }

        private void findIPRoutes(string[] chunks)
        {
            foreach (String chunk in chunks)
            {
                String[] words = chunk.Split(' ');
                try
                {
                    if (words[1].ToLower().Contains("classless"))
                    {
                        makeIPRoutes(chunk);
                    }
                }
                catch (Exception e)
                {
                    //No second word
                    //Console.WriteLine("Exception: " + e.Message);
                    //Console.ReadKey();
                }
            }
        }

        private void makeIPRoutes(string chunk)
        {
            String[] lines = chunk.Trim().Split('\n');
            foreach (String line in lines)
            {
                try
                {
                    String[] words = line.Split(' ');
                    if (words[1] == "route" && words[2] != "vrf" && !words[4].Contains("Null") && !words[4].Contains("Vlan"))
                    {
                        Vlan vlan = findVlanThroughRoute(words[2], words[3], words[4]);
                    }
                }
                catch (Exception e)
                {
                    //no second word
                    //Console.WriteLine("Exception: " + e.Message);
                    //Console.ReadKey();
                }
            }
        }

        private Vlan findVlanThroughRoute(string starterIP, string starterMask, string routeIP)
        {
            int starterIPInt = (int)IPstringToInt(starterIP);
            int starterMaskInt = (int)IPstringToInt(starterMask);
            string prefix = findPrefix(starterIPInt, starterMaskInt);
            return new Vlan();

        }
        public Int64 IPstringToInt(String stringIP)
        {
            string[] stringIPSegments = stringIP.Split('.');
            List<int> intIPSegments = stringIPtoIntIP(stringIPSegments);
            Int64 intIP = Convert.ToInt64((intIPSegments[0] * Math.Pow(2, 24)) + (intIPSegments[1] * Math.Pow(2, 16)) +
                (intIPSegments[2] * Math.Pow(2, 8)) + intIPSegments[3]);
            return intIP;
        }

        public string findPrefix(Int64 IP, Int64 MASK)
        {
            Int64 prefixInt = IP & MASK;
            List<int> temp = new List<int>();
            temp.Add((int)((prefixInt / Math.Pow(2, 24)) % (Math.Pow(2, 8))));
            temp.Add((int)((prefixInt / Math.Pow(2, 16)) % (Math.Pow(2, 8))));
            temp.Add((int)((prefixInt / Math.Pow(2, 8)) % (Math.Pow(2, 8))));
            temp.Add((int)(prefixInt % Math.Pow(2, 8)));
            return temp[0] + "." + temp[1] + "." + temp[2] + "." + temp[3];
        }

        public List<int> stringIPtoIntIP(string[] stringSegments)
        {
            List<int> intSegments = new List<int>();
            foreach (string stringSegment in stringSegments)
            {
                int intSegment = int.Parse(stringSegment);
                intSegments.Add(intSegment);
            }
            return intSegments;
        }

        private void findInterfaces(string[] chunks)
        {
            foreach (String chunk in chunks)
            {
                String[] words = chunk.Trim().Split(' ');
                if (words[0] == "interface")
                {
                    if (words[1].ToLower().Contains("vlan"))
                    {
                        Vlan vlan = new Vlan(chunk);
                        if (vlan.ip != null)
                        {
                            vlans.Add(vlan);
                        }
                    }
                    else
                    {
                        Interface inter = new Interface(chunk);

                        if (inter.ip != null)
                        {
                            interfaces.Add(inter);
                        }
                    }
                }
            }
        }
    }
}