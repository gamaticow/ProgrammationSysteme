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
                EditBackupCommand.RaiseCanExecuteChanged();
            }
        }
        private string _sourceDirectory;
        public string sourceDirectory
        {
            get
            {
                return _sourceDirectory;
            }
            set
            {
                _sourceDirectory = value;
                EditBackupCommand.RaiseCanExecuteChanged();
            }
        }
        private string _targetDirectory;
        public string targetDirectory
        {
            get
            {
                return _targetDirectory;
            }
            set
            {
                _targetDirectory = value;
                EditBackupCommand.RaiseCanExecuteChanged();
            }
        }

        private object _selectedType;
        public object selectedType
        {
            get
            {
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                EditBackupCommand.RaiseCanExecuteChanged();
            }
        }

        private object _backupType;
        public object backupType
        {
            get
            {
                return _backupType;
            }
            set
            {
                _backupType = value;
                EditBackupCommand.RaiseCanExecuteChanged();
            }
        }

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
        public ObservableCollection<object> backupTypeList { get; set; }
        public ICommand DeleteBackupCommand { get; private set; }
        public ExecuteBackupCommand ExecuteBackupCommand { get; private set; }
        public PauseBackupCommand PauseBackupCommand { get; private set; }
        public InterruptBackupCommand InterruptBackupCommand { get; private set; }

        public InfoBackupViewModel(BackupWork BackupWorkSelected)
        {
            this.BackupWorkSelected = BackupWorkSelected;

            EditBackupCommand = new EditBackupCommand(this);
            DeleteBackupCommand = new DeleteBackupCommand(this);
            ExecuteBackupCommand = new ExecuteBackupCommand(this);
            PauseBackupCommand = new PauseBackupCommand(this);
            InterruptBackupCommand = new InterruptBackupCommand(this);

            name = BackupWorkSelected.name;
            sourceDirectory = BackupWorkSelected.sourceDirectory;
            targetDirectory = BackupWorkSelected.targetDirectory;
            backupType = BackupWorkSelected.backupType;

            backupTypeList = new ObservableCollection<object>();
            foreach (BackupType backupType in Enum.GetValues(typeof(BackupType)))
            {
                var l = new { Name = Translate(backupType.ToString()), Type = backupType };
                backupTypeList.Add(l);
                if (BackupWorkSelected.backupType == backupType)
                {
                    selectedType = l;
                }
            }

            BackupWorkSelected.Subscribe(this);
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
            ExecuteBackupCommand.RaiseCanExecuteChanged();
            PauseBackupCommand.RaiseCanExecuteChanged();
            InterruptBackupCommand.RaiseCanExecuteChanged();
        }
    }
}
