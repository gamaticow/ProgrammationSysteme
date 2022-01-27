using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EasySave.Model
{
    class DifferentialBackupWork : BackupWork
    {
        public override void ExecuteBackup()
        {

            if (!System.IO.Directory.Exists(targetDirectory))
            {
                System.IO.Directory.CreateDirectory(targetDirectory);
            }

            // To copy all the files in one directory to another directory. 
            // Get the files in the source folder. (To recursively iterate through 
            // all subfolders under the current directory, see 
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously 
            //       in this code example. 
            if (System.IO.Directory.Exists(sourceDirectory))
            {
                string[] files = System.IO.Directory.GetFiles(sourceDirectory);

                // Copy the files and overwrite destination files if they already exist. 
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = System.IO.Path.GetFileName(s);
                    FileInfo sourceFile = new FileInfo(Path.Combine(sourceDirectory, fileName));
                    FileInfo destFile = new FileInfo(Path.Combine(targetDirectory, fileName));
                    String destFilestr = System.IO.Path.Combine(targetDirectory, fileName);
                    if (destFile.Exists)
                    {

                        if (sourceFile.LastWriteTime > destFile.LastWriteTime)
                        {
                            // now you can safely overwrite it
                            sourceFile.CopyTo(destFile.FullName, true);
                        }
                    }
                    else
                    {
                        System.IO.File.Copy(s, destFilestr, true);
                    }
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }

            // Keep console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
