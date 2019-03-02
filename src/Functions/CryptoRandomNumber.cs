using System;
using System.Security.Cryptography;

namespace CryptoStringAPI.Functions
{
    class CryptoRandomNumber : RandomNumberGenerator
    {
        private static RandomNumberGenerator r;

        public CryptoRandomNumber()
        {
            r = RandomNumberGenerator.Create();
        }

        public override void GetBytes(byte[] data)
        {
            r.GetBytes(data);
        }

        public static double NextDouble()
        {
            byte[] b = new byte[4];
            if (r == null)
                r = RandomNumberGenerator.Create();
            r.GetBytes(b);
            return (double)BitConverter.ToInt32(b, 0);
        }

        public static uint NextUInt()
        {
            byte[] b = new byte[4];
            if (r == null)
                r = RandomNumberGenerator.Create();
            r.GetBytes(b);
            return BitConverter.ToUInt32(b, 0);
        }

        public static int Next(long minValue, long maxValue)
        {
            try
            {
                long range = maxValue - minValue;
                return Math.Abs((int)((long)Math.Floor(NextDouble() * range) + minValue));
            }
            catch
            {
                try
                {
                    return Math.Abs((int)((long)Math.Floor(NextDouble() * Int16.MaxValue) + minValue));
                }
                catch
                {
                    return Int16.MaxValue;
                }
            }
        }

        public static int Next()
        {
            return Next(0, Int32.MaxValue);
        }

        public static int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
    }
}
