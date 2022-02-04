using EasySave.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModel
{
    class MenuViewModel : BaseViewModel
    {

        public BackupWork Selected { get; set; }
        public ObservableCollection<BackupWork> BackupWorksList { get; set; }

        public MenuViewModel()
        {
            BackupWorksList = new ObservableCollection<BackupWork>(Model.Model.Instance.backupWorks);
        }

        public override void SetTranslation()
        {
            
        }
    }
}
