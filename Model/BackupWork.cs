using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace EasySave.Model
{
    abstract class BackupWork
    {
        public String name;
        public String sourceDirectory;
        public String targetDirectory;
        public BackupType backupType { get; protected set; }
        public abstract void ExecuteBackup();
        protected void ExecuteBackup(DirectoryInfo source, DirectoryInfo target)
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
                string[] sourcefiles = System.IO.Directory.GetFiles(source.ToString());
                string[] targetfiles = System.IO.Directory.GetFiles(target.ToString());

                // Copy the files and overwrite destination files if they already exist. 
                foreach (string s in sourcefiles)
                {
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = System.IO.Path.GetFileName(s);
                    FileInfo sourceFile = new FileInfo(Path.Combine(source.ToString(), fileName));
                    FileInfo destFile = new FileInfo(Path.Combine(target.ToString(), fileName));
                    string destFilestr = System.IO.Path.Combine(target.ToString(), fileName);

                    if (destFile.Exists)
                    {
                        if (sourceFile.LastWriteTime > destFile.LastWriteTime && backupType == BackupType.DIFFERENTIAL)
                        {
                            // now you can safely overwrite it
                            sourceFile.CopyTo(destFile.FullName, true);
                        }
                        else if (backupType == BackupType.FULL)
                        {
                            sourceFile.CopyTo(destFile.FullName, true);
                        }
                    }
                    else
                    {
                        System.IO.File.Copy(s, destFilestr, true);
                    }
                }
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                    ExecuteBackup(diSourceSubDir, nextTargetSubDir);
                }
                if (backupType == BackupType.FULL)
                {
                    foreach (string t in targetfiles)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(t);
                        FileInfo sourceFile = new FileInfo(Path.Combine(source.ToString(), fileName));

                        if (!sourceFile.Exists)
                        {
                            File.Delete(t);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
        }

        protected void Log(string value)
        {

        }

    }
}
