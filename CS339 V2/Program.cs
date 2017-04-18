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
                    string name = getName(file);
                    StringBuilder bldr = new StringBuilder();
                    StreamReader rdr = new StreamReader(file);
                    while (rdr.Peek() >= 0)
                    {
                        bldr.Append(rdr.ReadLine());
                    }
                    routers.Add(new Router(name, bldr.ToString()));
                }
            }
            return routers;
        }

        private static string getName(string file)
        {
            string[] paths = file.Split('\\');
            return paths[paths.Count() - 1];
        }
    }
}
