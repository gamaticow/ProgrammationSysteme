using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class DeleteBackupCommand : ICommand
    {
        private InfoBackupViewModel viewModel;

        public DeleteBackupCommand(InfoBackupViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Model.Model.Instance.DeleteBackup(viewModel.BackupWorkSelected);
        }
    }
}
