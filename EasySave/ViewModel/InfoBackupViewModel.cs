using EasySave.Model;
using EasySave.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel
{
    class InfoBackupViewModel : BaseViewModel
    {
        
        public BackupWork BackupWorkSelected { get; set; }
        public string name { get; set; }
        public string sourceDirectory { get; set; }
        public string targetDirectory { get; set; }
        public string backupType { get; set; }

        public ICommand EditBackupCommand { get; private set; }
        public ICommand DeleteBackupCommand { get; private set; }
        public ICommand ExecuteBackupCommand { get; private set; }

        public InfoBackupViewModel(BackupWork BackupWorkSelected)
        {
            this.BackupWorkSelected = BackupWorkSelected;
            name = BackupWorkSelected.name;
            sourceDirectory = BackupWorkSelected.sourceDirectory;
            targetDirectory = BackupWorkSelected.targetDirectory;
            backupType = BackupWorkSelected.backupType.ToString();

            EditBackupCommand = new EditBackupCommand(this);
            DeleteBackupCommand = new DeleteBackupCommand(this);
            ExecuteBackupCommand = new ExecuteBackupCommand(this);
        }

        public override void SetTranslation()
        {
        }
    }
}
