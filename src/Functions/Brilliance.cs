using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class Brilliance
    {
        public static void Save(Bitmap bmp, string filename)
        {
            try
            {
                ImageFormat imf = ImageFormat.Png;

                string path = Path.GetDirectoryName(filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!String.IsNullOrWhiteSpace(Path.GetExtension(filename)))
                {
                    switch (Path.GetExtension(filename.ToLower()))
                    {
                        case ".bmp":
                            {
                                imf = ImageFormat.Bmp;
                                break;
                            }
                        case ".gif":
                            {
                                imf = ImageFormat.Gif;
                                break;
                            }
                        case ".jpg":
                        case ".jpeg":
                            {
                                imf = ImageFormat.Jpeg;
                                break;
                            }
                        case ".tif":
                        case ".tiff":
                            {
                                imf = ImageFormat.Tiff;
                                break;
                            }
                        case ".wmf":
                            {
                                imf = ImageFormat.Wmf;
                                break;
                            }
                    }
                }
                else
                {
                    filename += ".png";
                }
                bmp.Save(filename, imf);
            }
            catch
            {

            }
        }

        public static Bitmap Compress(List<byte> l, bool rotate = true)
        {
            int count = 0;
            int R = 0;
            int G = 0;
            int B = 0;

            int i = l.Count % 3;
            if (i != 0)
            {
                i = 3 - i;
                byte n = 255;
                for (int c = 1; c <= i; c++)
                {
                    l.Add(n);
                }
            }
            double square = Math.Sqrt(l.Count / 3) + 1;

            Bitmap bmp = new Bitmap((int)square + 2, (int)square + 2);
            int row = 0;
            int column = 0;

            for (row = 1; row <= square; row++)
            {
                for (column = 1; column <= square; column++)
                {
                    if (count < l.Count)
                    {
                        R = l[count++];
                        G = l[count++];
                        B = l[count++];
                        bmp.SetPixel(row, column, (Color.FromArgb(255, R, G, B)));
                    }
                }
            }

            if (rotate.Equals(true))
            {
                bmp = Rotate(bmp);
            }
            return bmp;
        }

        public static Bitmap Compress(string txt, bool rotate=true)
        {
            int count = 0;
            int R = 0;
            int G = 0;
            int B = 0;

            List<byte> l = Encoding.ASCII.GetBytes(txt).ToList();

            int i = l.Count % 3;
            if (i != 0)
            {
                i = 3 - i;
                byte n = 255;
                for (int c = 1; c <= i; c++)
                {
                    l.Add(n);
                }
            }
            double square = Math.Sqrt(l.Count / 3) + 1;

            Bitmap bmp = new Bitmap((int)square + 2, (int)square + 2);
            int row = 0;
            int column = 0;

            for (row = 1; row <= square; row++)
            {
                for (column = 1; column <= square; column++)
                {
                    if (count < l.Count)
                    {
                        R = l[count++];
                        G = l[count++];
                        B = l[count++];
                        bmp.SetPixel(row, column, (Color.FromArgb(255, R, G, B)));
                    }
                }
            }

            if (rotate.Equals(true))
            {
                bmp = Rotate(bmp);
            }

            return bmp;
        }


        public static string Decompress(Bitmap bmp, bool rotate = true)
        {
            if (rotate.Equals(true))
            {
                bmp = Unrotate(bmp);
            }
            List<int> l = new List<int>();

            for (int row = 1; row < bmp.Width; row++)
            {
                for (int column = 1; column < bmp.Height; column++)
                {
                    Color c = bmp.GetPixel(row, column);
                    l.Add(c.R);
                    l.Add(c.G);
                    l.Add(c.B);
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach (int c in l)
            {
                if (c != 255)
                {
                    if (c != 0)
                    {
                        sb.Append((Char)c);
                    }
                }
                else
                {
                    break;
                }
            }

            return sb.ToString();
        }

        private static Bitmap Rotate(Bitmap bmp)
        {
            if ((bmp.Width % 16).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
            else if ((bmp.Width % 15).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            else if ((bmp.Width % 14).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate180FlipXY);
            else if ((bmp.Width % 13).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
            else if ((bmp.Width % 12).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            else if ((bmp.Width % 11).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate270FlipX);
            else if ((bmp.Width % 10).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate270FlipXY);
            else if ((bmp.Width % 9).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate270FlipY);
            else if ((bmp.Width % 8).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            else if ((bmp.Width % 7).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            else if ((bmp.Width % 6).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
            else if ((bmp.Width % 5).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
            else if ((bmp.Width % 4).Equals(0))
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipNone);
            else if ((bmp.Width % 3).Equals(0))
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            else if ((bmp.Width % 2).Equals(0))
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            else
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bmp;
        }

        private static Bitmap Unrotate(Bitmap bmp)
        {
            if ((bmp.Width % 16).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
            else if ((bmp.Width % 15).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            else if ((bmp.Width % 14).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate180FlipXY);
            else if ((bmp.Width % 13).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
            else if ((bmp.Width % 12).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            else if ((bmp.Width % 11).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
            else if ((bmp.Width % 10).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
            else if ((bmp.Width % 9).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            else if ((bmp.Width % 8).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            else if ((bmp.Width % 7).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate270FlipY);
            else if ((bmp.Width % 6).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate270FlipXY);
            else if ((bmp.Width % 5).Equals(0))
                bmp.RotateFlip(RotateFlipType.Rotate270FlipX);
            else if ((bmp.Width % 4).Equals(0))
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipNone);
            else if ((bmp.Width % 3).Equals(0))
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            else if ((bmp.Width % 2).Equals(0))
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            else
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bmp;
        }
    }
}
