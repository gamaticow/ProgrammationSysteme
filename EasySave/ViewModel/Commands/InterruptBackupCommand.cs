using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class InterruptBackupCommand : DelegateCommandBase
    {
        private InfoBackupViewModel infoBackupViewModel;
        private MenuViewModel menuViewModel;

        public InterruptBackupCommand(InfoBackupViewModel viewModel)
        {
            infoBackupViewModel = viewModel;
        }

        public InterruptBackupCommand(MenuViewModel viewModel)
        {
            menuViewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            if (infoBackupViewModel != null)
            {
                return infoBackupViewModel.BackupWorkSelected.State == Model.BackupStateEnum.ACTIVE || infoBackupViewModel.BackupWorkSelected.State == Model.BackupStateEnum.PAUSE;
            }
            else if (menuViewModel != null)
            {
                return menuViewModel.Selected != null && (menuViewModel.Selected.BackupWork.State == Model.BackupStateEnum.ACTIVE || menuViewModel.Selected.BackupWork.State == Model.BackupStateEnum.PAUSE);
            }
            return false;
        }

        protected override void Execute(object parameter)
        {
            if (infoBackupViewModel != null)
            {
                infoBackupViewModel.BackupWorkSelected.Interupt();
            }
            else if (menuViewModel != null)
            {
                menuViewModel.Selected.BackupWork.Interupt();
            }
        }
    }
}
