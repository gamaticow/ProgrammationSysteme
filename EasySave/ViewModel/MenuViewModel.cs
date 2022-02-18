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
    class MenuViewModel : BaseViewModel, IObserver<BackupState>
    {
        public string Tcreate_backup { get; set; }
        public string Tinfo { get; set; }
        public string Tsequential_execution { get; set; }
        public MenuBackupWork Selected { get; set; }

        private List<MenuBackupWork> _backupWorkList;
        public ObservableCollection<MenuBackupWork> BackupWorksList
        {
            get
            {
                return new ObservableCollection<MenuBackupWork>(_backupWorkList);
            }
            set
            {
                
            }
        }
        public ICommand SequentialExecutionCommand { get; private set; }

        public MenuViewModel()
        {
            _backupWorkList = new List<MenuBackupWork>();
            foreach(BackupWork backupWork in Model.Model.Instance.backupWorks)
            {
                _backupWorkList.Add(new MenuBackupWork() { BackupWork = backupWork, Name = backupWork.name, Progress = 0, Color = "#198754" });
                backupWork.Subscribe(this);
            }

            SequentialExecutionCommand = new SequentialExecutionCommand(this);
        }

        public override void SetTranslation()
        {
            Tcreate_backup = Translate("create_backup");
            Tinfo = Translate("get_info");
            Tsequential_execution = Translate("sequential_execution");
        }

        public void OnCompleted()
        {}

        public void OnError(Exception error)
        {}

        public void OnNext(BackupState value)
        {
            MenuBackupWork backupWork = null;
            foreach(MenuBackupWork bw in _backupWorkList)
            {
                if(bw.Name == value.Name)
                {
                    backupWork = bw;
                    break;
                }
            }
            if(backupWork == null)
            {
                return;
            }

            string color = "#198754";
            if (value.State == "PAUSE")
            {
                color = "#ffc107";
            }
            else if (value.State == "INTERRUPTED")
            {
                color = "#dc3545";
            }

            backupWork.Progress = value.Progression;
            backupWork.Color = color;

            OnPropertyChanged(nameof(BackupWorksList));
        }
    }

    internal class MenuBackupWork
    {
        public BackupWork BackupWork { get; set; }
        public string Name { get; set; }
        public int Progress { get; set; }
        public string Color { get; set; }
    }
}
