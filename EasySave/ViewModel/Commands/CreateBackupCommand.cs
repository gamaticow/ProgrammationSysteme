using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasySave.Model;

namespace EasySave.ViewModel.Commands
{
    class CreateBackupCommand: ICommand
    {
        private CreateBackupViewModel viewModel;

        public CreateBackupCommand(CreateBackupViewModel viewModel)
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
            Model.Model.Instance.CreateBackupWork(viewModel.name, viewModel.sourceDirectory, viewModel.targetDirectory, viewModel.backupType);
        }
    }
}
