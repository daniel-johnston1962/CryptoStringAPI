using System.IO;
using System.IO.Compression;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class Zip
    {

        public static byte[] Compress(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            using (MemoryStream msb = new MemoryStream(bytes))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (GZipStream gs = new GZipStream(ms, CompressionMode.Compress))
                    {
                        msb.CopyTo(gs);
                    }

                    return ms.ToArray();
                }
            }
        }

        public static string Decompress(byte[] bytes)
        {
            using (MemoryStream msb = new MemoryStream(bytes))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (GZipStream gs = new GZipStream(msb, CompressionMode.Decompress))
                    {
                        gs.CopyTo(ms);
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

    }
}
