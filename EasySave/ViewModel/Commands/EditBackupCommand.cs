using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
            return viewModel.name != null && viewModel.name.Length > 0 && viewModel.name != viewModel.BackupWorkSelected.name;
        }

        protected override void Execute(object parameter)
        {
            viewModel.BackupWorkSelected.name = viewModel.name;
        }
    }
}
