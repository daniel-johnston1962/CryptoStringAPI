using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class Crypto
    {
        public enum CryptoType : int
        {
            aes,
            des,
            rc2,
            rijndael,
            tripledes
        }

        //''' AES - Key must be 32 characters (256 bytes) and IV must be 16 characters (128 bytes)
        //''' DES - Key must be 8 characters (64 bytes) and IV must be 8 characters (64 bytes)
        //''' RC2 - Key must be 16 characters (128 bytes) and IV must be 8 characters (64 bytes)
        //''' Rijndael - Key must be 32 characters (256 bytes) and IV must be 16 characters (128 bytes)
        //''' Triple DES - Key must be 24 characters (192 bytes) and IV must be 8 characters (64 bytes)

        public static string Encrypt(CryptoType cryptoMethod, string txt)
        {
            string retVal = String.Empty;
            try
            {
                //CryptoCollection cc = CryptoCollection.GetCryptoCollectionInfoByMethod(cryptoMethod);

                //retVal = Encrypt(cryptoMethod, txt, cc);
            }
            catch
            {

            }
            return retVal;
        }

        public static string Encrypts(string cryptoMethod, string txt)
        {
            string retVal = String.Empty;
            try
            {
                CryptoType ct;
                Enum.TryParse(cryptoMethod, out ct);

                CryptoCollection cc = CryptoCollection.GetCryptoCollectionInfoByMethod(cryptoMethod);

                //retVal = Encrypt(cryptoMethod, txt, cc);
            }
            catch
            {

            }
            return retVal;
        }

        public static string Encrypt(string cryptoMethod, string txt)
        {
            string retVal = String.Empty;
            try
            {
                CryptoType ct;
                Enum.TryParse(cryptoMethod, true, out ct);

                CryptoCollection cc = CryptoCollection.GetCryptoCollectionInfoByMethod(cryptoMethod);
                string key = genRandomText(cc.Key);
                string iv = genRandomText(cc.IV);
                string e = Encrypt(ct, txt, key, iv);

                int i = (int)e.FirstOrDefault();

                key = String.Join(String.Empty, key.Reverse().ToArray());
                iv = String.Join(String.Empty, iv.Reverse().ToArray());

                if (i < e.Length)
                    e = e.Substring(0, i) + key + iv + e.Substring(i);
                else
                    e += key + iv;

                retVal = "[" + cc.ID + "]" + e;
            }
            catch
            {

            }
            return retVal;
        }

        public static string Encrypt(CryptoType cryptoMethod, string txt, string cryptoKey, string cryptoIV)
        {
            string retValue = String.Empty;

            try
            {
                byte[] key = Encoding.UTF8.GetBytes(cryptoKey);
                byte[] iv = Encoding.UTF8.GetBytes(cryptoIV);

                ICryptoTransform cTransform = null;

                switch (cryptoMethod)
                {
                    case CryptoType.aes:
                        using (Aes aes = Aes.Create())
                        {
                            if ((checkSize(aes.Key.Length, key.Length).Equals(true)) & (checkSize(aes.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = aes.CreateEncryptor(key, iv);
                            }
                        }
                        break;
                    case CryptoType.des:
                        using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                        {
                            if ((checkSize(des.Key.Length, key.Length).Equals(true)) & (checkSize(des.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = des.CreateEncryptor(key, iv);
                            }
                        }
                        break;
                    case CryptoType.rc2:
                        using (RC2 rc2 = RC2.Create())
                        {
                            if ((checkSize(rc2.Key.Length, key.Length).Equals(true)) & (checkSize(rc2.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = rc2.CreateEncryptor(key, iv);
                            }
                        }
                        break;
                    case CryptoType.rijndael:
                        using (Rijndael rij = Rijndael.Create())
                        {
                            if ((checkSize(rij.Key.Length, key.Length).Equals(true)) & (checkSize(rij.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = rij.CreateEncryptor(key, iv);
                            }
                        }
                        break;
                    case CryptoType.tripledes:
                        using (TripleDESCryptoServiceProvider triDES = new TripleDESCryptoServiceProvider())
                        {
                            if ((checkSize(triDES.Key.Length, key.Length).Equals(true)) & (checkSize(triDES.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = triDES.CreateEncryptor(key, iv);
                            }
                        }
                        break;
                }
                if (cTransform != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, cTransform, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(txt);
                            }
                            retValue = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                    cTransform.Dispose();
                }
                else
                {
                    throw new CryptographicException();
                }
            }
            catch (CryptographicException cex)
            {
                throw new ApplicationException("Cryptographic Encryption Exception", cex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encrypt Exception", ex);
            }
            return retValue;
        }

        public static string Decrypt(string txt)
        {
            string retVal = String.Empty;
            try
            {
                CryptoCollection cc = CryptoCollection.GetCryptoCollectionInfoByID(txt.Substring(1, 3));

                CryptoType ct;
                Enum.TryParse(cc.Method, out ct);

                int i = (int)Convert.ToChar(txt.Substring(5, 1));
                string d = String.Empty;
                string key = String.Empty;
                string iv = String.Empty;

                if (i < (txt.Length - cc.Key - cc.IV))
                {
                    d = txt.Substring(5, i) + txt.Substring(5 + i + cc.Key + cc.IV);
                    key = txt.Substring(5 + i, cc.Key);
                    iv = txt.Substring(5 + i + cc.Key, cc.IV);
                }
                else
                {
                    d = txt.Substring(5, txt.Length - cc.Key - cc.IV - 5);
                    key = txt.Substring(txt.Length - cc.Key - cc.IV, cc.Key);
                    iv = txt.Substring(txt.Length - cc.IV);
                }

                key = String.Join(String.Empty, key.Reverse().ToArray());
                iv = String.Join(String.Empty, iv.Reverse().ToArray());

                retVal = Decrypt(ct, d, key, iv);
            }
            catch
            {

            }
            return retVal;
        }

        public static string Decrypt(CryptoType cryptoMethod, string txt, string cryptoKey, string cryptoIV)
        {
            string retValue = String.Empty;

            try
            {
                byte[] key = Encoding.UTF8.GetBytes(cryptoKey);
                byte[] iv = Encoding.UTF8.GetBytes(cryptoIV);
                byte[] array = Convert.FromBase64String(txt);

                ICryptoTransform cTransform = null;

                switch (cryptoMethod)
                {
                    case CryptoType.aes:
                        using (Aes aes = Aes.Create())
                        {
                            if ((checkSize(aes.Key.Length, key.Length).Equals(true)) & (checkSize(aes.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = aes.CreateDecryptor(key, iv);
                            }
                        }
                        break;
                    case CryptoType.des:
                        using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                        {
                            if ((checkSize(des.Key.Length, key.Length).Equals(true)) & (checkSize(des.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = des.CreateDecryptor(key, iv);
                            }
                        }
                        break;
                    case CryptoType.rc2:
                        using (RC2 rc2 = RC2.Create())
                        {
                            if ((checkSize(rc2.Key.Length, key.Length).Equals(true)) & (checkSize(rc2.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = rc2.CreateDecryptor(key, iv);
                            }
                        }
                        break;
                    case CryptoType.rijndael:
                        using (Rijndael rij = Rijndael.Create())
                        {
                            if ((checkSize(rij.Key.Length, key.Length).Equals(true)) & (checkSize(rij.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = rij.CreateDecryptor(key, iv);
                            }
                        }
                        break;
                    case CryptoType.tripledes:
                        using (TripleDESCryptoServiceProvider trides = new TripleDESCryptoServiceProvider())
                        {
                            if ((checkSize(trides.Key.Length, key.Length).Equals(true)) & (checkSize(trides.IV.Length, iv.Length).Equals(true)))
                            {
                                cTransform = trides.CreateDecryptor(key, iv);
                            }
                        }
                        break;
                }

                if (cTransform != null)
                {
                    using (MemoryStream ms = new MemoryStream(array))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, cTransform, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                retValue = sr.ReadToEnd();
                            }
                        }
                    }
                    cTransform.Dispose();
                }
                else
                {
                    throw new CryptographicException();
                }
            }
            catch (CryptographicException cex)
            {
                throw new ApplicationException("Cryptographic Decryption Exception", cex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Decrypt Exception", ex);
            }
            return retValue;
        }

        private static bool checkSize(int typeLength, int thisLength)
        {
            bool tf = false;

            try
            {
                if (typeLength.Equals(thisLength))
                    tf = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception", ex);
            }
            return tf;
        }

        private static String genRandomText(int length)
        {
            Random random = new Random((int)(DateTime.Now.Ticks) + BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            String randomString = String.Empty;

            for (int i = 0; i < length; i++)
            {
                int min = random.Next(33, 38);
                int max = random.Next(120, 126);
                randomString += Convert.ToChar(random.Next(min, max));
            }
            return randomString;
        }

    }
}
