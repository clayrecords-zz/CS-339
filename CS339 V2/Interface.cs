using System;
using System.Collections.Generic;

namespace CS339_V2
{
    class Interface
    {
        public String name;
        public String ip;
        public String mask;
        public String prefix;
        public Int64 ipInt;
        public Int64 maskInt;
        public Int64 prefixInt;

        public Interface(String chunk)
        {
            String[] lines = chunk.Trim().Split('\n');
            findName(lines);
            findIPAddress(lines);

            if (ip != null)
            {
                findPrefix();
            }
        }

        public void findName(String[] lines)
        {
            String[] words = lines[0].Split(' ');
            name = words[1];
        }

        public void findIPAddress(string[] lines)
        {
            foreach (String line in lines)
            {
                String[] words = line.Trim().Split(' ');
                if (words[0] == "ip" && words[1] == "address" && words.Length != 5)
                {
                    ip = words[2];
                    mask = words[3];
                }
            }
        }

        public Int64 IPstringToInt(String stringIP)
        {
            string[] stringIPSegments = stringIP.Split('.');
            List<int> intIPSegments = stringIPtoIntIP(stringIPSegments);
            Int64 intIP = Convert.ToInt64((intIPSegments[0] * Math.Pow(2, 24)) + (intIPSegments[1] * Math.Pow(2, 16)) +
                (intIPSegments[2] * Math.Pow(2, 8)) + intIPSegments[3]);
            return intIP;
        }

        public void findPrefix()
        {
            ipInt = IPstringToInt(ip);
            maskInt = IPstringToInt(mask);
            prefixInt = ipInt & maskInt;

            List<int> temp = new List<int>();
            temp.Add((int)((prefixInt / Math.Pow(2, 24)) % (Math.Pow(2, 8))));
            temp.Add((int)((prefixInt / Math.Pow(2, 16)) % (Math.Pow(2, 8))));
            temp.Add((int)((prefixInt / Math.Pow(2, 8)) % (Math.Pow(2, 8))));
            temp.Add((int)(prefixInt % Math.Pow(2, 8)));

            prefix = temp[0] + "." + temp[1] + "." + temp[2] + "." + temp[3];
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
    }
}