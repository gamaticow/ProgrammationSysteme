using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Cryptosoft
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            string inputKey = args[0];
            string sourcePath = args[1];
            string destPath = args[2];

            byte[] key = Encoding.Unicode.GetBytes(inputKey);
            byte[] b;

            FileInfo fi = new FileInfo(sourcePath);
            FileInfo fo = new FileInfo(destPath);

            // Delete the file if it exists.
            if (!fi.Exists)
            {
                Environment.Exit(-1);
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

            stopwatch.Start();
            byte[] encrypted = EncryptOrDecrypt(b, key);
            stopwatch.Stop();

            using (FileStream fs = fo.OpenWrite())
            {
                // Add some information to the file.
                fs.Write(encrypted, 0, encrypted.Length);
            }
            
            int encryptTime = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
            Environment.Exit(encryptTime);
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
