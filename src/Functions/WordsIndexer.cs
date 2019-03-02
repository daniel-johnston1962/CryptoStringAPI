using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class WordsIndexer
    {
        public static string constructWordIndexer(string s, bool reg = true)
        {
            List<string> l = new List<string>(s.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            List<string> d = new List<string>(l.OrderBy(x => x.Length).ThenByDescending(x => x).Distinct().Reverse());
            StringBuilder sb = new StringBuilder();
            List<char> cl = new List<char>(s.ToArray());

            var cv = cl.Select((value, index) => new { value, index })
                    .Where(a => cl.Any(ss => string.Equals(a.value, '\r')))
                    .Select(a => a.index)
                    .ToList();
            if (reg.Equals(true))
            {
                // regular numbers
                sb.Append(string.Join("|", cv.Select(x => x).ToArray()));
            }
            else
            {
                // ascii char for numbers
                sb.Append(string.Join("|", cv.Select(x => x.ToString("X")).ToArray()));
            }
            sb.Append(Environment.NewLine);

            foreach (string ds in d)
            {
                var v = l.Select((value, index) => new { value, index })
                    .Where(a => l.Any(ss => string.Equals(a.value, ds)))
                    .Select(a => a.index)
                    .ToList();

                sb.Append(ds + " " + string.Join(",", v.Select(x => x).ToArray()));
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();

        }

        public static string deconstructWordIndexer(string s, bool reg = true)
        {
            List<string> l = new List<string>(s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            String[] rec = { " " };

            int i = 0;
            bool tf = false;

            if (!l[0].Contains(' '))
            {
                i = 1;
                tf = true;
            }
            while (i < l.Count)
            {
                string[] sl = l[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
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
                    rec[ilsIndex] = sl[0];

                }
                i++;
            }

            List<char> strC = new List<char>(string.Join(" ", rec.Select(x => x.ToString())).ToCharArray());
            if (tf.Equals(true))
            {
                List<string> ill = new List<string>(l[0].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));

                l.Clear();
                int crIndex = 0;
                int illSize = strC.Count;
                if (reg.Equals(false))
                {
                    foreach (string sill in ill)
                    {
                        crIndex = int.Parse(sill, NumberStyles.HexNumber);
                        illSize = strC.Count;
                        addCRLF(ref strC, crIndex, illSize);
                    }

                    ill.Clear();
                }
                else
                {
                    foreach (string sill in ill)
                    {
                        crIndex = 0;
                        int.TryParse(sill, out crIndex);
                        illSize = strC.Count;
                        addCRLF(ref strC, crIndex, illSize);
                    }
                }
            }
            return string.Join("", strC.Select(x => x.ToString())).TrimEnd(' ');
        }
        
        private static void addCRLF(ref List<char> strC, int crIndex, int illSize)
        {
            int diff = 0;
            if (crIndex >= illSize)
            {
                diff = crIndex - illSize + 5;
                strC.AddRange(Enumerable.Repeat((char)32, diff));
                illSize = strC.Count;
            }
            strC.Insert(crIndex, '\r');

            if (crIndex + 1 < illSize)
            {
                if (strC[crIndex + 1].Equals(' '))
                    strC[crIndex + 1] = '\n';
                else
                    strC.Insert(crIndex + 1, '\n');
            }
            else
            {
                strC.Add('\n');
            }
        }
    }
}
