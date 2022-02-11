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
                CreateBackupCommand.RaiseCanExecuteChanged();
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
                CreateBackupCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(sourceDirectory));
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
                CreateBackupCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(targetDirectory));
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
                CreateBackupCommand.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<object> backupTypeList { get; set; }

        public CreateBackupCommand CreateBackupCommand { get; private set; }

        public SelectFolderCommand SelectFolderCommand { get; private set; }

        public CreateBackupViewModel()
        {
            CreateBackupCommand = new CreateBackupCommand(this);
            SelectFolderCommand = new SelectFolderCommand(this);
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
