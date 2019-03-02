using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class CharIndexer
    {
        public static string constructWordIndexer(string str)
        {
            StringBuilder sb = new StringBuilder();
            List<char> cl = new List<char>(str.ToArray());
            List<char> dl = new List<char>(cl.Distinct());

            foreach (char c in dl)
            {
                var cv = cl.Select((value, index) => new { value, index })
                        .Where(a => cl.Any(s => string.Equals(a.value, c)))
                        .Select(a => a.index)
                        .ToList();
                sb.Append(Convert.ToInt32(c).ToString() + ":" + string.Join(",", cv.Select(x => x).ToArray()));
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public static string deconstructWordIndexer(string str)
        {
            int i = 0;
            String[] rec = { " " };
            List<string> l = new List<string>(str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

            while (i < l.Count)
            {
                string[] sl = l[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                string[] il = sl[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string ils in il)
                {

                    int recSize = rec.Length;
                    int ilsIndex = Convert.ToInt32(ils);
                    if (ilsIndex >= recSize)
                    {
                        Array.Resize(ref rec, ilsIndex + 2);
                        recSize = rec.Length;
                        rec[recSize - 1] = " ";
                        rec[recSize - 2] = " ";
                    }
                    rec[ilsIndex] = Convert.ToChar(Convert.ToInt32(sl[0])).ToString();

                }
                i++;
            }

            return string.Join("", rec.Select(x => x.ToString())).TrimEnd();
        }
    }
}
