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
    class MenuViewModel : BaseViewModel
    {

        public BackupWork Selected { get; set; }
        public string Tcreate_backup { get; set; }
        public string Tinfo { get; set; }
        public string Tsequential_execution { get; set; }
        public ObservableCollection<BackupWork> BackupWorksList { get; set; }
        public ICommand SequentialExecutionCommand { get; private set; }

        public MenuViewModel()
        {
            BackupWorksList = new ObservableCollection<BackupWork>(Model.Model.Instance.backupWorks);

            SequentialExecutionCommand = new SequentialExecutionCommand(this);
        }

        public override void SetTranslation()
        {
            Tcreate_backup = Translate("create_backup");
            Tinfo = Translate("get_info");
            Tsequential_execution = Translate("sequential_execution");
        }
    }
}
