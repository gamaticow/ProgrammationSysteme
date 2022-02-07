using EasySave.ViewModel.Commands;
using System;
using System.Collections.Generic;
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
        public string TButtonCreate { get; set; }
        public string name { get; set; }
        public string sourceDirectory { get; set; }
        public string targetDirectory { get; set; }
        public string backupType { get; set; }

        public ICommand CreateBackupCommand { get; private set; }

        public CreateBackupViewModel()
        {
            CreateBackupCommand = new CreateBackupCommand(this);
        }

        public override void SetTranslation()
        {
            TLabelBackupName = Translate("label_backup_name");
            TLabelSourceDirectory = Translate("label_source_directory");
            TLabelTargetDirectory = Translate("label_target_directory");
            TLabelBackupType = Translate("label_backup_type");
            TButtonCreate = Translate("button_create");
        }
    }
}
