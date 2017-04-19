using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS339_V2
{
    class Visualization
    {
        public Visualization(List<Router> routers)
        {
            try
            {
                StreamWriter sw = new StreamWriter("C:\\Users\\MPRecords\\Desktop\\Graph.txt");
                sw.WriteLine("Graph{");
                List<String> connections = makeGVConnections(routers);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: Vis - " + e.Message);
                Console.ReadKey();
            }
        }


        private List<string> makeGVConnections(List<Router> routers)
        {
            List<String> connections = new List<String>();
            foreach (Router router in routers)
            {
                foreach (String prefix in router.connectedRouters.Keys)
                {
                    String x = (router.name + " -- " + router.connectedRouters[prefix].name +
                                   " [label = \"" + prefix + " \"];");

                    addUniqueConnections(connections, x, prefix);
                }
            }
            return connections;
        }


        private void addUniqueConnections(List<string> connections, string x, String prefix)
        {
            Boolean duplicate = false;
            if (connections.Count == 0)
            {
                connections.Add(x);
            }
            for (int c = 0; c < connections.Count; c++)
            {
                if (connections[c].Contains(prefix))
                {
                    duplicate = true;
                }
            }
            if (duplicate == false)
            {
                connections.Add(x);
            }
        }
    }
}
