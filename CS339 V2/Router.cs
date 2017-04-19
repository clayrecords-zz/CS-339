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
        public List<Vlan> routes;
        public SortedDictionary<string, Router> connectedRouters = new SortedDictionary<string, Router>();
        public SortedDictionary<string, Vlan> connectedVlans = new SortedDictionary<string, Vlan>();

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
                        Vlan vlan = new Vlan();
                        vlan.populate(words[2], words[3], words[4]);
                        routes.Add(vlan);
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

        public void route()
        {
            foreach (Vlan route in routes)
            {
                foreach (Router connectedRouter in connectedRouters.Values)
                {
                    foreach (Vlan vlan in connectedRouter.vlans)
                    {
                        if (vlan.ip == route.bridgeIP)
                        {
                            foreach (Vlan vla in connectedRouter.vlans)
                            {
                                if (vla.prefix == route.prefix)
                                {
                                    vlans.Add(vla);
                                }
                            }
                        }
                    }
                }
            }
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
                        Vlan vlan = new Vlan();
                        vlan.populate(chunk);
                        if (vlan.ip != null)
                        {
                            vlans.Add(vlan);
                        }
                    }
                    else
                    {
                        Interface inter = new Interface();
                        inter.populate(chunk);
                        if (inter.ip != null)
                        {
                            interfaces.Add(inter);
                        }
                    }
                }
            }
        }
        public void addConnection(String prefix, Router router)
        {
            connectedRouters.Add(prefix, router);
        }
    }
}