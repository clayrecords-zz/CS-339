using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CS339_V2
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<Router> routers = getRouters();
            findConnections(routers);
            findRoutedVlans(routers);
        }

        private static void findConnections(List<Router> routers)
        {
            for (int i = 0; i < routers.Count; i++)
            {
                for (int j = 0; j < routers[i].interfaces.Count; j++)
                {
                    for (int x = i + 1; x < routers.Count; x++)
                    {
                        for (int y = 0; y < routers[x].interfaces.Count; y++)
                        {
                            //compare ij to xy
                            if (routers[i].interfaces[j].prefix == routers[x].interfaces[y].prefix)
                            {
                                routers[i].addConnection(routers[i].interfaces[j].prefix, routers[x]);
                                routers[x].addConnection(routers[x].interfaces[y].prefix, routers[i]);
                            }
                        }
                    }
                }
            }
        }

        private static List<Router> getRouters()
        {
            FolderBrowserDialog b = new FolderBrowserDialog();
            List<Router> routers = new List<Router>();
            if (b.ShowDialog() == DialogResult.OK)
            {
                var folderName = b.SelectedPath;
                Console.WriteLine(folderName);
                foreach (var file in Directory.EnumerateFiles(folderName, "*.txt"))
                {
                    Router r = createRouter(file);
                    routers.Add(r);
                }
            }
            return routers;
        }

        public static void findRoutedVlans(List<Router> routers)
        {
            foreach (Router router in routers)
            {
                router.route();
            }
        }

        private static Router createRouter(string file)
        {
            string name = getName(file);
            StringBuilder bldr = new StringBuilder();
            StreamReader rdr = new StreamReader(file);
            while (rdr.Peek() >= 0)
            {
                bldr.Append(rdr.ReadLine());
            }
            return (new Router(name, bldr.ToString()));
        }

        private static string getName(string file)
        {
            string[] paths = file.Split('\\');
            return paths[paths.Count() - 1];
        }
    }
}
