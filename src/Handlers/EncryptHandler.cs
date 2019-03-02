using CryptoStringAPI.Functions;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using static CryptoStringAPI.Functions.Crypto;

namespace CryptoStringAPI.Handlers
{
    public class EncryptCommand
    {
        private string _Method { get; set; }
        private string _PictureType { get; set; }
        public string Method
        {
            get => _Method;
            set => _Method = value.Trim().ToLower();
        }
        public string PictureType
        {
            get => _PictureType;
            set => _PictureType = value.Trim().ToLower();
        }
        public string Text { get; set; } 
    }
    public class EncryptResult
    {
        public string PictureType { get; set; }
        public string EncryptedPicture { get; set; }
    }
    public class EncryptHandler
    {
        public EncryptHandler()
        {

        }

        public EncryptResult Handle(EncryptCommand command)
        {
            EncryptResult result = new EncryptResult();

            if (command == null || string.IsNullOrWhiteSpace(command?.Text))
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(command.Method) || !Enum.IsDefined(typeof(CryptoType), command.Method))
            {
                command.Method = "aes";
            }

            List<string> pt = new List<string>() { "bmp", "png" };
            if (string.IsNullOrWhiteSpace(command.PictureType) || !pt.Contains(command?.PictureType))
            {
                command.PictureType = "png";
            }

            result.EncryptedPicture = Encrypt(command);
            result.PictureType = command.PictureType;

            return result;
        }

        private string Encrypt(EncryptCommand command)
        {
            string rtn = string.Empty;

            var tokens = Token.Tokens();
            var img = Brilliance.Compress(
                         Convert.ToBase64String(
                            Zip.Compress(
                               Crypto.Encrypt(
                                 command.Method, Convert.ToBase64String(
                                    Zip.Compress(
                                        Obfuscation.Obfuscate(
                                            tokens.ReplaceWords(command.Text)))))))); 

            if (img != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    if (command.PictureType == "png")
                    {
                        img.Save(stream, ImageFormat.Png);
                    }
                    else
                    {
                        img.Save(stream, ImageFormat.Bmp);
                    }
                    byte[] imageBytes = stream.ToArray();
                    rtn = Convert.ToBase64String(imageBytes);
                }
            }

            return rtn;
        }
    }
}
