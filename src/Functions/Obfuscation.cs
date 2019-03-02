using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class Obfuscation
    {
        public static string FlipText(string s)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                List<char> l = new List<char>(s.ToArray());
                int mid = 79;
                int x = 0;

                l.Reverse();

                foreach (char c in l)
                {
                    int i = Convert.ToInt32(c);
                    if (i >= 32 && i <= 126)
                    {
                        if (i < mid)
                        {
                            x = mid - i + mid;
                        }
                        else
                        {
                            x = i - mid;
                            x = mid - x;
                        }
                        sb.Append(Convert.ToChar(x));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            catch
            {

            }
            return sb.ToString();
        }

        private static int RandomNumber()
        {
            Random r = new Random(CryptoRandomNumber.Next());
            int min = r.Next(1, 64);
            int max = r.Next(200, 254);
            return r.Next(min, max);
        }

        private static string Shift(string s, int num)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                List<char> l = new List<char>(s.ToArray());
                foreach (char c in l)
                {
                    int ch = Convert.ToInt32(c);
                    if (ch <= 255)
                    {
                        int i = ch + num;
                        if (num > 0)
                        {
                            if (i > 255)
                                i -= 255;
                        }
                        else
                        {
                            if (i < 0)
                                i += 255;
                        }
                        if (i == 0)
                            sb.Append(Convert.ToChar(255));
                        else
                            sb.Append(Convert.ToChar(i));
                    }
                    else
                    {
                        sb.Append(c.ToString());
                    }
                }
            }
            catch
            {

            }
            return sb.ToString();
        }

        public static string ShiftText(string s)
        {
            Random r = new Random(CryptoRandomNumber.Next());
            int rndmin = r.Next(7, 44);
            int rndmax = r.Next(66, 127);
            int rnd = r.Next(rndmin, rndmax);

            string hex = rnd.ToString("X2");
            return hex + Obfuscation.Shift(s, rnd);
        }

        public static string UnshiftText(string s)
        {
            string hex = s.Substring(0, 2);
            int i = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            s = s.Substring(2);
            return Obfuscation.Shift(s, -i);
        }

        private static string Lock(string s, string key, bool tf)
        {
            StringBuilder sb = new StringBuilder();
            List<char> l = new List<char>(s.ToArray());
            List<char> k = new List<char>(key.ToArray());
            int k_num = 0;
            int i = 0;
            foreach (char c in l)
            {
                if (k_num >= k.Count())
                    k_num = 0;
                if (tf.Equals(true))
                    i = Convert.ToInt32(c) + Convert.ToInt32(k[k_num]);
                else
                    i = Convert.ToInt32(c) - Convert.ToInt32(k[k_num]);
                k_num++;
                sb.Append(Convert.ToChar(i));
            }

            return sb.ToString();
        }

        public static string LockText(string s)
        {
            string key = CreateKey();
            return "<<" + key + ">> " + Obfuscation.Lock(s, key, true);
        }

        public static string UnlockText(string s)
        {
            int i = s.IndexOf(">> ");
            string key = s.Substring(2, i - 2);
            return Obfuscation.Lock(s.Substring(i + 3), key, false);
        }

        public static string Obfuscate(string s)
        {
            return FlipText(LockText(FlipText(LockText(FlipText(ShiftText(s))))));
        }

        public static string Unobfuscate(string s)
        {
            return UnshiftText(FlipText(UnlockText(FlipText(UnlockText(FlipText(s))))));
        }

        private static string CreateKey()
        {
            StringBuilder sb = new StringBuilder();
            Random r = new Random(CryptoRandomNumber.Next());
            int rndmin = r.Next(7, 23);
            int rndmax = r.Next(88, 112);
            int rnd = r.Next(rndmin, rndmax);

            for (int i = 0; i < rnd; i++)
            {
                int minmin = r.Next(32, 67);
                int minmax = r.Next(68, 127);
                int maxmin = r.Next(128, 255);
                int maxmax = r.Next(256, 312);
                int min = r.Next(minmin, minmax);
                int max = r.Next(maxmin, maxmax);
                int c = r.Next(min, max);
                sb.Append((char)c);
            }

            return sb.ToString();
        }

        private static char[] chars = "abcdefghijklmnopqrstuvwxyz0123456789+_)(*&^%$#@![]|}{;:,./?><ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public static string RandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            Random r = null;
            char[] randomChars = Shuffle<char>(chars.ToList()).ToArray();
            for (int j = 0; j < length; j++)
            {
                uint randomInt = CryptoRandomNumber.NextUInt();
                int num1 = CryptoRandomNumber.Next();
                int num2 = CryptoRandomNumber.Next();
                if (num1 < num2)
                    r = new Random(CryptoRandomNumber.Next(num1, num2));
                else
                    r = new Random(CryptoRandomNumber.Next(num2, num1));
                int min = r.Next(1, 10);
                int max = r.Next(77, 90);
                if (max > 87)
                    max = 87;
                char cc = randomChars[randomInt % r.Next(min, max)];
                sb.Append(cc);
            }
            return sb.ToString();
        }

        public static List<T> Shuffle<T>(List<T> t)
        {
            return t.OrderBy(x => new Random(CryptoRandomNumber.Next()).Next()).ToList();
        }

        public static string Bloat(string s)
        {
            List<char> l = new List<char>(s.ToArray());
            StringBuilder sb = new StringBuilder();
            Random random = new Random(CryptoRandomNumber.Next());
            int r = random.Next(4, 93);
            char rc = (char)(33 + r);
            sb.Append(rc.ToString());
            foreach (char c in l)
            {
                sb.Append(c.ToString());
                sb.Append(RandomString(r));
            }

            return sb.ToString();
        }

        public static string Unbloat(string s)
        {
            List<char> l = new List<char>(s.ToArray());
            StringBuilder sb = new StringBuilder();
            int r = (int)l[0] - 33;
            int count = 0;
            l.RemoveAt(0);
            foreach (char c in l)
            {
                if (count % (r + 1) == 0)
                {
                    sb.Append(c.ToString());
                }
                count++;
            }

            return sb.ToString();
        }

        public static string RandomlyHide(string s)
        {

            List<char> l = new List<char>(s.ToArray());
            StringBuilder sb = new StringBuilder();
            Random random = new Random(CryptoRandomNumber.Next());
            int r = random.Next(16, 93);
            char rc = (char)(33 + r);
            sb.Append(rc.ToString());
            int mod = 10;

            foreach (char c in l)
            {
                int num = r % mod;
                sb.Append(c.ToString());
                sb.Append(RandomString(num));
                mod--;
                if (mod == 0)
                    mod = 10;
            }

            return sb.ToString();
        }

        public static string UnrandomlyHide(string s)
        {

            List<char> l = new List<char>(s.ToArray());
            StringBuilder sb = new StringBuilder();
            int r = (int)l[0] - 33;
            int count = 0;
            l.RemoveAt(0);
            int mod = 10;
            sb.Append(l[0].ToString());

            do
            {
                int num = r % mod;
                count += num + 1;
                if (count < l.Count)
                    sb.Append(l[count]);
                mod--;
                if (mod == 0)
                    mod = 10;

            } while (count < l.Count);

            return sb.ToString();
        }

    }
}
