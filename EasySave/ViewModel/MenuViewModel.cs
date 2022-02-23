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
        public string Tparallel_execution { get; set; }
        public string TSelectedBackupWork { get; set; }

        private MenuBackupWork _selected;
        public MenuBackupWork Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                UpdateViewCommand.RaiseCanExecuteChanged();
                ExecuteBackupCommand.RaiseCanExecuteChanged();
                PauseBackupCommand.RaiseCanExecuteChanged();
                InterruptBackupCommand.RaiseCanExecuteChanged();
            }
        }

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
        public ICommand ParallelExecutionCommand { get; private set; }
        public ICommand BackupWorkDoubleClickCommand { get; private set; }
        public ExecuteBackupCommand ExecuteBackupCommand { get; private set; }
        public PauseBackupCommand PauseBackupCommand { get; private set; }
        public InterruptBackupCommand InterruptBackupCommand { get; private set; }

        public MenuViewModel()
        {
            ParallelExecutionCommand = new ParallelExecutionCommand(this);
            BackupWorkDoubleClickCommand = new BackupWorkDoubleClickCommand(this);
            ExecuteBackupCommand = new ExecuteBackupCommand(this);
            PauseBackupCommand = new PauseBackupCommand(this);
            InterruptBackupCommand = new InterruptBackupCommand(this);

            _backupWorkList = new List<MenuBackupWork>();
            foreach (BackupWork backupWork in Model.Model.Instance.backupWorks)
            {
                _backupWorkList.Add(new MenuBackupWork() { BackupWork = backupWork, Name = backupWork.name, Progress = 0, Color = "#198754", Image = "../Resources/green_play.png" });
                backupWork.Subscribe(this);
            }
        }

        public override void SetTranslation()
        {
            Tcreate_backup = Translate("create_backup");
            Tinfo = Translate("get_info");
            Tparallel_execution = Translate("parallel_execution");
            TSelectedBackupWork = Translate("selected_backup_work");
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
            string image = null;
            if (value.State == "ACTIVE")
            {
                image = "../Resources/green_play.png";
            }
            else if (value.State == "PAUSE")
            {
                color = "#ffc107";
                image = "../Resources/orange_pause.png";
            }
            else if (value.State == "INTERRUPTED")
            {
                color = "#dc3545";
                image = "../Resources/red_stop.png";
            }
            else if (value.State == "END")
            {
                image = null;
            }

            backupWork.Progress = value.Progression;
            backupWork.Color = color;
            backupWork.Image = image;

            OnPropertyChanged(nameof(BackupWorksList));
            ExecuteBackupCommand.RaiseCanExecuteChanged();
            PauseBackupCommand.RaiseCanExecuteChanged();
            InterruptBackupCommand.RaiseCanExecuteChanged();
        }
    }

    internal class MenuBackupWork
    {
        public BackupWork BackupWork { get; set; }
        public string Name { get; set; }
        public int Progress { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
    }
}
