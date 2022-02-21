using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasySave.Model;
using System.Reflection;

namespace EasySave.ViewModel.Commands
{
    class EditBackupCommand : DelegateCommandBase
    {
        private InfoBackupViewModel viewModel;

        public EditBackupCommand(InfoBackupViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        protected override bool CanExecute(object parameter)
        {
            Type t = viewModel.selectedType.GetType();
            PropertyInfo info = t.GetProperty("Type");
            BackupType backupType = (BackupType)info.GetValue(viewModel.selectedType);
            BackupType backupTypeSelected = viewModel.BackupWorkSelected.backupType;
            return (viewModel.name != null && viewModel.name.Length > 0 && viewModel.name != viewModel.BackupWorkSelected.name) || (viewModel.sourceDirectory != null && viewModel.sourceDirectory.Length > 0 && viewModel.sourceDirectory != viewModel.BackupWorkSelected.sourceDirectory) || (viewModel.targetDirectory != null && viewModel.targetDirectory.Length > 0 && viewModel.targetDirectory != viewModel.BackupWorkSelected.targetDirectory) || (backupType.ToString().Length > 0 && backupType != backupTypeSelected);
        }

        protected override void Execute(object parameter)
        {
            viewModel.BackupWorkSelected.name = viewModel.name;
            viewModel.BackupWorkSelected.sourceDirectory = viewModel.sourceDirectory;
            viewModel.BackupWorkSelected.targetDirectory = viewModel.targetDirectory;

            Type t = viewModel.selectedType.GetType();
            PropertyInfo info = t.GetProperty("Type");
            BackupType backupType = (BackupType)info.GetValue(viewModel.selectedType);
            viewModel.BackupWorkSelected.backupType = backupType;
        }
    }
}
