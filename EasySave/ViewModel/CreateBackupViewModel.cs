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
        }
    }
}
