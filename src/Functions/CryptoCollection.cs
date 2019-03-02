using System.Collections.Generic;
using System.Linq;

namespace CryptoStringAPI.Functions
{
    public class CryptoCollection
    {
        public string Method { get; set; }
        public string ID { get; set; }
        public int Key { get; set; }
        public int IV { get; set; }

        public CryptoCollection(string method, string id, int key, int iv)
        {
            Method = method;
            ID = id;
            Key = key;
            IV = iv;
        }
        
        //''' AES - Key must be 32 characters (256 bytes) and IV must be 16 characters (128 bytes)
        //''' DES - Key must be 8 characters (64 bytes) and IV must be 8 characters (64 bytes)
        //''' RC2 - Key must be 16 characters (128 bytes) and IV must be 8 characters (64 bytes)
        //''' Rijndael - Key must be 32 characters (256 bytes) and IV must be 16 characters (128 bytes)
        //''' Triple DES - Key must be 24 characters (192 bytes) and IV must be 8 characters (64 bytes)

        public static List<CryptoCollection> CryptoCollectionInfo = new List<CryptoCollection>()
        {
            new CryptoCollection("aes", "aes", 32, 16),
            new CryptoCollection("des", "des", 8, 8),
            new CryptoCollection("rc2", "rc2", 16, 8),
            new CryptoCollection("rijndael", "rij", 32, 16),
            new CryptoCollection("tripledes", "tds", 24, 8)
        };

        public static List<string> GetCryptoCollectionMethods()
        {
            return CryptoCollectionInfo.Select(x => x.Method).ToList();
        }

        public static CryptoCollection GetCryptoCollectionInfoByID(string id)
        {
            return CryptoCollectionInfo.FirstOrDefault(x => x.ID == id);
        }

        public static CryptoCollection GetCryptoCollectionInfoByMethod(string m)
        {
            return CryptoCollectionInfo.FirstOrDefault(x => x.Method == m);
        }
    }
}
