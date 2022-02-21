using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class ExecuteBackupCommand : DelegateCommandBase
    {
        private InfoBackupViewModel infoBackupViewModel;
        private MenuViewModel menuViewModel;

        public ExecuteBackupCommand(InfoBackupViewModel viewModel)
        {
            infoBackupViewModel = viewModel;
        }

        public ExecuteBackupCommand(MenuViewModel viewModel)
        {
            menuViewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            if(infoBackupViewModel != null)
            {
                return infoBackupViewModel.BackupWorkSelected.State != Model.BackupStateEnum.ACTIVE;
            }
            else if(menuViewModel != null)
            {
                return menuViewModel.Selected != null && menuViewModel.Selected.BackupWork.State != Model.BackupStateEnum.ACTIVE;
            }
            return false;
        }

        protected override void Execute(object parameter)
        {
            if (infoBackupViewModel != null)
            {
                infoBackupViewModel.BackupWorkSelected.ExecuteBackup();
            }
            else if (menuViewModel != null)
            {
                menuViewModel.Selected.BackupWork.ExecuteBackup();
            }
        }
    }
}
