using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class Matrix
    {
        private static int upperNum = 0;
        private static int lowerNum = 0;
        private static int processStrAmt = 0;
        private static int processSubStrAmt = 0;
        private static string hexString = String.Empty;
        private static string printHex = String.Empty;

        private static void initializeAscii()
        {
            upperNum = 256;
            lowerNum = 16;
            hexString = "X";
            processStrAmt = 2;
            processSubStrAmt = 1;
            printHex = "000";
        }

        private static void initializeUnicode()
        {
            upperNum = 65536;
            lowerNum = 256;
            hexString = "X2";
            processStrAmt = 4;
            processSubStrAmt = 2;
            printHex = "00000";
        }

        public static string PrintAsciiMatrix(int seed)
        {
            initializeAscii();
            return printMatrix(seed);
        }

        public static string PrintUnicodeMatrix(int seed)
        {
            initializeUnicode();
            return printMatrix(seed);
        }

        public static string CreateAsciiMatrix(string txt)
        {
            initializeAscii();
            return matrixProcess(txt);
        }

        public static string CreateUnicodeMatrix(string txt)
        {
            initializeUnicode();
            return matrixProcess(txt);
        }

        public static string UncreateAsciiMatrix(string txt)
        {
            initializeAscii();
            return dematrixProcess(txt);
        }

        public static string UncreateUnicodeMatrix(string txt)
        {
            initializeUnicode();
            return dematrixProcess(txt);
        }

        private static string matrixProcess(string s)
        {
            string Row = String.Empty;
            String Col = String.Empty;
            int row = 0;
            int col = 0;
            int seed = CryptoRandomNumber.Next();
            string hex = seed.ToString("X8");
            
            StringBuilder sb = new StringBuilder();
            List<int> numbers = new List<int>();
            List<List<int>> matrix = new List<List<int>>();

            CreateRandomNumbers(seed, ref numbers);
            CreateMatrix(numbers, ref matrix); 
            char[] sc = s.ToCharArray();

            foreach (char c in sc)
            {
                if ((int)c < upperNum)
                {
                    row = 0;
                    col = 0;
                    findIndex(matrix, ref row, ref col, c);

                    Row = convertRC(row);
                    Col = convertRC(col);
                    sb.Append(Row + Col);
                }
                else
                {
                    sb.Append(c.ToString());
                }
            }
            sb.Insert(0, hex);
            return sb.ToString();
        }

        private static string dematrixProcess(string s)
        {
            string Row = String.Empty;
            String Col = String.Empty;
            int row = 0;
            int col = 0;

            StringBuilder sb = new StringBuilder();
            List<int> numbers = new List<int>();
            List<List<int>> matrix = new List<List<int>>();

            string hex = s.Substring(0, 8);
            int seed = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            s = s.Substring(8);

            CreateRandomNumbers(seed, ref numbers);
            CreateMatrix(numbers, ref matrix);

            for (int i = 0; i < s.Length; i += processStrAmt)
            {
                if (processSubStrAmt.Equals(1))
                {
                    int fc = Convert.ToChar(s.Substring(i, 1));
                    if (fc > 255)
                    {
                        sb.Append(s.Substring(i, 1));
                        i--;
                        continue;
                    }
                }
                string ss = s.Substring(i, processStrAmt);
                row = Convert.ToInt32(ss.Substring(0, processSubStrAmt), 16);
                col = Convert.ToInt32(ss.Substring(processSubStrAmt, processSubStrAmt), 16);
                int rc = matrix[row][col];
                sb.Append(Convert.ToChar(rc).ToString());
            }
            return sb.ToString();
        }

        public static void findIndex(List<List<int>> am, ref int row, ref int col, char c)
        {
            int ci = (int)c;
            foreach (List<int> ro in am)
            {
                col = ro.FindIndex(a => a.Equals(ci));
                if (col >= 0)
                {
                    break;
                }
                row++;
            }
        }

        private static void CreateRandomNumbers(int seed, ref List<int> numbers)
        {
            Random rnd = new Random(seed);
            numbers = Enumerable.Range(0, upperNum).OrderBy(rn => rnd.Next()).Distinct().ToList();
        }

        private static void CreateMatrix(List<int> numbers, ref List<List<int>> matrix)
        {
            for (int i = 0; i < upperNum; i += lowerNum)
            {
                List<int> subList = new List<int>();
                subList.AddRange(numbers.Skip(i).Take(lowerNum));
                matrix.Add(subList);
            }
        }

        private static string convertRC(int i)
        {
            return i.ToString(hexString);
        }

        private static int unconvertRC(string s)
        {
            return Convert.ToInt32(s, 16);
        }

        private static string printMatrix(int seed)
        {
            StringBuilder sb = new StringBuilder();
            List<int> numbers = new List<int>();
            CreateRandomNumbers(seed, ref numbers);

            List<List<int>> matrix = new List<List<int>>();
            CreateMatrix(numbers, ref matrix);

            foreach (List<int> am in matrix)
            {
                foreach (int i in am)
                {
                    sb.Append(i.ToString(printHex) + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
