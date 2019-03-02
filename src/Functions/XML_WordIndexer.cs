using CryptoStringAPI.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CryptoTest
{
    public class XML_WordIndexer
    {
        public static string constructXML_WordIndexer(string s, bool dax = false)
        {           
            var xDoc = new XDocument();
            var main = new XElement("main");
            var fields = new XElement("fields");
            var spchars = new XElement("special_chars");

            List<char> cl = new List<char>(s);
            fields.Add(new XAttribute("numOfElements", cl.Count));
            List<char> dl = new List<char>(cl.Distinct().OrderBy(x => x));
            List<char> dl2 = new List<char>();

            int numOfWords = 0;

            //foreach (char c in dl)
            //{
            //    if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c == '\'')))
            //        dl2.Add(c);
            //}

            Parallel.ForEach(dl, (char c) =>
            {
                if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c == '\'')))
                    dl2.Add(c);
            });

            //foreach (char c in dl2)
            //{
            //    var cv = cl.Select((value, index) => new { value, index })
            //            .Where(a => cl.Any(t => string.Equals(a.value, c)))
            //            .Select(a => a.index)
            //            .ToList();
            //    var field = new XElement("field");
            //    field.Add(new XAttribute("value", Convert.ToInt32(c).ToString()));
            //    field.Add(new XElement("index", string.Join(",", cv.Select(x => x).ToArray())));
            //    spchars.Add(field);
            //}

            Parallel.ForEach<char>(dl2, (char c) =>
            {
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
                spchars.Add(field);

            });

            fields.Add(spchars);
            var words = new XElement("words");
            
            // Remove spaces from list
            dl2.Remove(Convert.ToChar(10));
            dl2.Remove(Convert.ToChar(13));
            dl2.Remove(Convert.ToChar(32));


            foreach (char c in dl2)
            {
                cl.RemoveAll(x => x.Equals(c));
            }

            string st = string.Join("", cl.ToArray());

            List<string> l = new List<string>(st.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            List<string> d = new List<string>(l.OrderBy(x => x.Length).ThenByDescending(x => x).Distinct().Reverse());

            //foreach (string sl in d)
            //{
            //    var cv = l.Select((value, index) => new { value, index })
            //            .Where(a => l.Any(t => string.Equals(a.value, sl)))
            //            .Select(a => a.index)
            //            .ToList();
            //    var field = new XElement("word");
            //    field.Add(new XAttribute("value", sl));
            //    field.Add(new XElement("index", string.Join(",", cv.Select(x => x).ToArray())));
            //    words.Add(field);
            //    numOfWords += cv.Count;
            //}

            Parallel.ForEach(d, (string sl) =>
            {
                var cv = l.Select((value, index) => new { value, index })
                        .Where(a => l.Any(t => string.Equals(a.value, sl)))
                        .Select(a => a.index)
                        .ToList();
                var field = new XElement("word");
                field.Add(new XAttribute("value", sl));
                if (dax == true)
                    field.Add(new XElement("index", string.Join(" ", cv.Select(x => Dax.ConvertNumberToDax((ulong)x)).ToArray())));
                else
                    field.Add(new XElement("index", string.Join(",", cv.Select(x => x).ToArray())));
                words.Add(field);
                numOfWords += cv.Count;
            });

            
            fields.Add(new XAttribute("numOfWords", numOfWords));
            fields.Add(words);
            main.Add(fields);
            xDoc.Add(main);

            return xDoc.ToString();
        }

        public static string deconstructXML_WordIndexer(string s, bool dax = false)
        {
            var xDoc = XDocument.Parse(s);

            var indexes = xDoc.Descendants("fields").Attributes("numOfWords").FirstOrDefault().Value;
            var elements = xDoc.Descendants("fields").Attributes("numOfElements").FirstOrDefault().Value;
            int t = 0;
            int.TryParse(indexes, out t);

            string[] val = new string[t];

            foreach (XElement xe in xDoc.Descendants("word"))
            {
                string index = xe.Attribute("value").Value;
                string value = xe.Value;

                if (dax == true)
                {
                    string[] il = xe.Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string ils in il)
                    {
                        int ilsIndex = (int)Dax.ConvertDaxToNumber(ils);
                        val[ilsIndex] = index;
                    }
                }
                else
                {
                    string[] il = xe.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string ils in il)
                    {
                        int tp = 0;
                        int.TryParse(ils, out tp);
                        val[tp] = index;
                    }
                }
            }
            List<char> letters = string.Join("", val.Select(x => x.ToString())).ToList();

            int numElements = 0;
            int.TryParse(elements.ToString(), out numElements);

            List<char> words = new List<char>();
            words.AddRange(Enumerable.Repeat((char)0, numElements));

            foreach (XElement xe in xDoc.Descendants("field"))
            {
                string index = xe.Attribute("value").Value;
                string value = xe.Value;
                
                string[] il = xe.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                int ti = 0;
                int.TryParse(index, out ti);
                char c = Convert.ToChar(ti);
                foreach (string ils in il)
                {
                    int tp = 0;
                    int.TryParse(ils, out tp);
                    words[tp] = c; 
                }
            }

            int indexLetters = 0;
            for (int indexWords = 0; indexWords < words.Count; indexWords++)
            {
                if (words[indexWords] == (char)0)
                {
                    words[indexWords] = letters[indexLetters];
                    indexLetters++;
                }
            }

            return string.Join("", words.Select(x => x.ToString()));

        }
    }
}
