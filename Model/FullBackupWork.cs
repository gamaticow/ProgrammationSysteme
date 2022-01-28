using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EasySave.Model
{
    class FullBackupWork : BackupWork
    {
        public override void ExecuteBackup()
        {
            var sourceDirectoryInfo = new DirectoryInfo(this.sourceDirectory);
            var targetDirectoryInfo = new DirectoryInfo(this.targetDirectory);

            // Stock the source directory files in an array variable
            string[] files = Directory.GetFiles(this.sourceDirectory);

            // Create a new target directory if it does not exist
            if (!Directory.Exists(this.targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // Loop over all files and copy each file
            foreach (string file in files)
            {
                File.Copy(file, $"{this.targetDirectory}{ Path.GetFileName(file)}", true);
            }

            // Copy each subdirectory and its files to the target directory
            foreach(var sourceSubdirectory in sourceDirectoryInfo.GetDirectories())
            {
                var targetSubdirectory = targetDirectoryInfo.CreateSubdirectory(sourceSubdirectory.Name);

                foreach (var file in sourceSubdirectory.GetFiles())
                {
                    file.CopyTo(Path.Combine(targetSubdirectory.FullName, file.Name), true);
                }
            }
        }
    }
}
