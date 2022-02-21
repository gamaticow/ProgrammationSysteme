using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class BackupWorkDoubleClickCommand : ICommand
    {
        private MenuViewModel viewModel;

        public BackupWorkDoubleClickCommand(MenuViewModel viewModel)
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
            viewModel.SelectedViewModel = new InfoBackupViewModel(viewModel.Selected.BackupWork);
        }
    }
}
