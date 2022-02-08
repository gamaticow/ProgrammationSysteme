using EasySave.Model;
using EasySave.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel
{
    class CreateBackupViewModel: BaseViewModel
    {
        public string TLabelBackupName { get; set; }
        public string TLabelSourceDirectory { get; set; }
        public string TLabelTargetDirectory { get; set; }
        public string TLabelBackupType { get; set; }
        public string name { get; set; }
        public string sourceDirectory { get; set; }
        public string targetDirectory { get; set; }
        public object selectedType { get; set; }
        public ObservableCollection<object> backupTypeList { get; set; }

        public ICommand CreateBackupCommand { get; private set; }

        public CreateBackupViewModel()
        {
            CreateBackupCommand = new CreateBackupCommand(this);
            backupTypeList = new ObservableCollection<object>();
            foreach (BackupType backupType in Enum.GetValues(typeof(BackupType)))
            {
                var l = new { Name = Translate(backupType.ToString()), Type = backupType };
                backupTypeList.Add(l);
            }
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
