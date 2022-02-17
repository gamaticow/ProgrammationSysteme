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
    class InfoBackupViewModel : BaseViewModel, IObserver<BackupState>
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

        private int _progress = 0;
        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        private string _progressColor = "#198754";
        public string ProgressColor
        {
            get
            {
                return _progressColor;
            }
            set
            {
                _progressColor = value;
                OnPropertyChanged(nameof(ProgressColor));
            }
        }

        public EditBackupCommand EditBackupCommand { get; private set; }
        public ICommand DeleteBackupCommand { get; private set; }
        public ICommand ExecuteBackupCommand { get; private set; }
        public ICommand PauseBackupCommand { get; private set; }
        public ICommand InterruptBackupCommand { get; private set; }

        public InfoBackupViewModel(BackupWork BackupWorkSelected)
        {
            this.BackupWorkSelected = BackupWorkSelected;

            BackupWorkSelected.Subscribe(this);

            name = BackupWorkSelected.name;
            sourceDirectory = BackupWorkSelected.sourceDirectory;
            targetDirectory = BackupWorkSelected.targetDirectory;
            backupType = BackupWorkSelected.backupType.ToString();

            EditBackupCommand = new EditBackupCommand(this);
            DeleteBackupCommand = new DeleteBackupCommand(this);
            ExecuteBackupCommand = new ExecuteBackupCommand(this);
            PauseBackupCommand = new PauseBackupCommand(this);
            InterruptBackupCommand = new InterruptBackupCommand(this);
        }

        public override void SetTranslation()
        {
            TLabelBackupName = Translate("label_backup_name");
            TLabelSourceDirectory = Translate("label_source_directory");
            TLabelTargetDirectory = Translate("label_target_directory");
            TLabelBackupType = Translate("label_backup_type");
        }

        public void OnCompleted()
        {}

        public void OnError(Exception error)
        {}

        public void OnNext(BackupState value)
        {
            Progress = value.Progression;
            if(value.State == "PAUSE")
            {
                ProgressColor = "#ffc107";
            }
            else if(value.State == "INTERRUPTED")
            {
                ProgressColor = "#dc3545";
            }
            else
            {
                ProgressColor = "#198754";
            }
        }
    }
}
