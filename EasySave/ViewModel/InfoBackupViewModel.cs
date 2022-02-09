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
        public string TLabelBackupName { get; set; }
        public string TLabelSourceDirectory { get; set; }
        public string TLabelTargetDirectory { get; set; }
        public string TLabelBackupType { get; set; }
        public BackupWork BackupWorkSelected { get; set; }

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                if(EditBackupCommand != null)
                    EditBackupCommand.RaiseCanExecuteChanged();
            }
        }
        public string sourceDirectory { get; set; }
        public string targetDirectory { get; set; }
        public string backupType { get; set; }

        public EditBackupCommand EditBackupCommand { get; private set; }
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
            TLabelBackupName = Translate("label_backup_name");
            TLabelSourceDirectory = Translate("label_source_directory");
            TLabelTargetDirectory = Translate("label_target_directory");
            TLabelBackupType = Translate("label_backup_type");
        }
    }
}
