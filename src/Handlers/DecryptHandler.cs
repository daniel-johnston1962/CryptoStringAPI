using CryptoStringAPI.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoStringAPI.Handlers
{
    public class DecryptCommand
    {
        public string EncryptedPicture { get; set; }
    }
    public class DecryptResult
    {
        public string Text { get; set; }
    }
    public class DecryptHandler
    {
        public DecryptHandler()
        {

        }

        public DecryptResult Handle(DecryptCommand command)
        {
            DecryptResult result = new DecryptResult();

            if (command == null && !string.IsNullOrWhiteSpace(command.EncryptedPicture))
            {
                return result;
            }

            result.Text = Decrypt(command);

            return result;
        }

        private string Decrypt(DecryptCommand command)
        {
            string rtn = string.Empty;
            Bitmap bmp = null;
            var tokens = Token.Tokens();

            byte[] imageBytes = Convert.FromBase64String(command.EncryptedPicture);
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                memoryStream.Position = 0;
                bmp = (Bitmap)Bitmap.FromStream(memoryStream);
            }

            if (bmp != null)
            {
                rtn = tokens.ReplaceTokens(
                        Obfuscation.Unobfuscate(
                            Zip.Decompress(
                                Convert.FromBase64String(
                                    Crypto.Decrypt(
                                        Zip.Decompress(
                                            Convert.FromBase64String(
                                                   Brilliance.Decompress(bmp))))))));
            }

            return rtn;
        }
    }
}
