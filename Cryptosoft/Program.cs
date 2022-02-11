using System;
using System.Text;
using System.IO;

namespace Cryptosoft
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = @"C:\Users\rasor\OneDrive\Documents\test\levik2.png";
            string destPath = @"C:\Users\rasor\OneDrive\Documents\test\levik2.png";
            string inputKey = "AZEFGBDNKCOIJGFDHKBGJLDNSK";
            byte[] key = Encoding.Unicode.GetBytes(inputKey);
            byte[] b;

            FileInfo fi = new FileInfo(sourcePath);
            FileInfo fo = new FileInfo(destPath);

            // Delete the file if it exists.
            if (!fi.Exists)
            {
                //Create the file.
                using (FileStream fs = fi.Create())
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                    //Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            
            //Open the stream and read it back.
            using (FileStream fs = fi.OpenRead())
            {
                b = new byte[fs.Length];
                //UTF8Encoding temp = new UTF8Encoding(true);
                fs.Read(b, 0, (int)fs.Length);
                /*while (fs.Read(b, 0, b.Length) > 0)
                {
                    fichierstr = temp.GetString(b);
                }*/
            }

            byte[] encrypted = EncryptOrDecrypt(b, key);

            using (FileStream fs = fo.OpenWrite())
            {
                // Add some information to the file.
                fs.Write(encrypted, 0, encrypted.Length);
            }
        }

        public static byte[] EncryptOrDecrypt(byte[] text, byte[] key)
        {
            byte[] xor = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                xor[i] = (byte)(text[i] ^ key[i % key.Length]);
            }
            return xor;
        }
    }
}
