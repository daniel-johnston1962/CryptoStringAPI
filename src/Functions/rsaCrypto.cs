using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class rsaCrypto
    {
        public static RSACryptoServiceProvider rsa;

        public static void AssignParameter()
        {
            const int PROVIDER_RSA_FULL = 1;
            const string CONTAINER_NAME = "TheContainer";
            const string PROVIDER_NAME = "Microsoft Strong Cryptographic Provider";
            CspParameters cspParams;
            cspParams = new CspParameters();
            cspParams.ProviderType = PROVIDER_RSA_FULL;
            cspParams.KeyContainerName = CONTAINER_NAME;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = PROVIDER_NAME;
            rsa = new RSACryptoServiceProvider(cspParams);
        }

        public static void AssignNewKey()
        {
            AssignParameter();

            //provide public and private RSA params
            using (StreamWriter writerPrivate = new StreamWriter(@"c:\temp\xmlkeys\privatekey.xml"))
            {
                string publicPrivateKeyXML = rsa.ToXmlString(true);
                writerPrivate.Write(publicPrivateKeyXML);
            }
            //provide public only RSA params
            using (StreamWriter writerPublic = new StreamWriter(@"c:\temp\xmlkeys\publickey.xml"))
            {
                string publicOnlyKeyXML = rsa.ToXmlString(false);
                writerPublic.Write(publicOnlyKeyXML);
            }

        }

        public static string Encrypt(string data2Encrypt)
        {
            AssignParameter();
            using (StreamReader reader = new StreamReader(@"c:\temp\xmlkeys\publickey.xml"))
            {
                string publicOnlyKeyXML = reader.ReadToEnd();
                rsa.FromXmlString(publicOnlyKeyXML);
            }

            //read plaintext, encrypt it to ciphertext
            byte[] plainbytes = Encoding.UTF8.GetBytes(data2Encrypt);
            byte[] cipherbytes = rsa.Encrypt(plainbytes, false);
            return Convert.ToBase64String(cipherbytes);
        }

        public static string Decrypt(string data2Decrypt)
        {
            AssignParameter();
            using (StreamReader reader = new StreamReader(@"c:\temp\xmlkeys\privatekey.xml"))
            {
                string publicPrivateKeyXML = reader.ReadToEnd();
                rsa.FromXmlString(publicPrivateKeyXML);
            }

            //read ciphertext, decrypt it to plaintext
            byte[] getpassword = Convert.FromBase64String(data2Decrypt);
            byte[] plain = rsa.Decrypt(getpassword, false);
            return Encoding.UTF8.GetString(plain);

        }
    }
}
