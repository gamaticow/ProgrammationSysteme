using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EasySave.Model;
using Prism.Commands;

namespace EasySave.ViewModel.Commands
{
    class CreateBackupCommand: DelegateCommandBase
    {
        private CreateBackupViewModel viewModel;

        public CreateBackupCommand(CreateBackupViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        protected override bool CanExecute(object parameter)
        {
            return viewModel.name != null && viewModel.name.Length > 0 && viewModel.sourceDirectory != null && viewModel.sourceDirectory.Length > 0 && viewModel.targetDirectory != null && viewModel.targetDirectory.Length > 0 && viewModel.selectedType != null;
        }

        protected override void Execute(object parameter)
        {
            Type t = viewModel.selectedType.GetType();
            PropertyInfo info = t.GetProperty("Type");
            BackupType backupType = (BackupType)info.GetValue(viewModel.selectedType);
            int result = Model.Model.Instance.CreateBackupWork(viewModel.name, viewModel.sourceDirectory, viewModel.targetDirectory, backupType);
            string error = Model.Model.Instance.language.Translate("error");
            if(result == 0)
            {
                viewModel.SelectedViewModel = new MenuViewModel();
                return;
            }
            else if(result == 1)
            {
                error = Model.Model.Instance.language.Translate("error_unknown_type");
            }
            else if (result == 2)
            {
                error = Model.Model.Instance.language.Translate("error_name_already_used");
            }
            else if (result == 3)
            {
                error = Model.Model.Instance.language.Translate("error_fields_empty");
            }

            MessageBox.Show(error, Model.Model.Instance.language.Translate("error_title"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
