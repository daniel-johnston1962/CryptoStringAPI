using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CryptoStringAPI.Functions
{
    public class XML_CharIndexer
    {
        public static string constructXML_CharIndexer(string s, bool dax = false)
        {
            List<char> cl = new List<char>(s);
            List<char> dl = new List<char>(cl.Distinct().OrderBy(x => x));

            var xDoc = new XDocument();
            var main = new XElement("main");
            var fields = new XElement("fields");
            fields.Add(new XAttribute("numOfElements", cl.Count));

            Parallel.ForEach(dl, (char c) => {
                var cv = cl.Select((value, index) => new { value, index })
                        .Where(a => cl.Any(t => string.Equals(a.value, c)))
                        .Select(a => a.index)
                        .ToList();
                var field = new XElement("field");
                field.Add(new XAttribute("value", Convert.ToInt32(c).ToString()));
                if (dax == true)
                    field.Add(new XElement("index", string.Join(" ", cv.Select(x => Dax.ConvertNumberToDax((ulong)x)).ToArray())));
                else
                    field.Add(new XElement("index", string.Join(",", cv.Select(x => x).ToArray())));
                fields.Add(field);

            });

            main.Add(fields);
            xDoc.Add(main);

            return xDoc.ToString();
        }

        public static string deconstructXML_CharIndexer(string s, bool dax = false)
        {
            var xDoc = XDocument.Parse(s);

            var indexes = xDoc.Descendants("fields").Attributes("numOfElements").FirstOrDefault().Value;
            int t = 0;
            int.TryParse(indexes, out t);

            var tc = (new int[t]).Select(x => ' ').ToList();

            foreach (XElement xe in xDoc.Descendants("field"))
            {
                string index = xe.Attribute("value").Value;
                string value = xe.Value;

                if (dax == true)
                {
                    string[] il = xe.Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string ils in il)
                    {
                        int ilsIndex = (int)Dax.ConvertDaxToNumber(ils);
                        tc[ilsIndex] = Convert.ToChar(Convert.ToInt32(index));
                    }
                }
                else
                {
                    string[] il = xe.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string ils in il)
                    {
                        int ilsIndex = 0;
                        int.TryParse(ils, out ilsIndex);
                        tc[ilsIndex] = Convert.ToChar(Convert.ToInt32(index));
                    }
                }
            }
            return string.Join("", tc.Select(x => x.ToString()));
        }
    }
}
