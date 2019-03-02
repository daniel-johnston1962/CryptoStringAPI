using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoStringAPI.Functions
{
    public class Dax
    {
        static public string ConvertNumberToDax(ulong num)
        {
            StringBuilder sb = new StringBuilder();
            ulong wholeNumber = 0;
            ulong remainder = 0;
            if (num >= 0)
            {
                do
                {
                    wholeNumber = num / 94;
                    remainder = num % 94;
                    sb.Append(Convert.ToChar(remainder + 33).ToString());
                    num = wholeNumber;
                } while (wholeNumber != 0);
            }
            return new string(sb.ToString().Reverse().ToArray());
        }

        static public ulong ConvertDaxToNumber(string s)
        {
            ulong total = 0;
            if (!string.IsNullOrWhiteSpace(s))
            {
                char[] arr = s.ToCharArray();
                int counter = arr.Length - 1;
                foreach (char c in arr)
                {
                    if (counter == 0)
                    {
                        total += Convert.ToUInt32(c) - 33;
                    }
                    else
                    {
                        total += (ulong)(Math.Pow(94, counter) * (Convert.ToInt32(c) - 33));
                        counter--;
                    }
                }
            }
            return total;
        }

    }
}
