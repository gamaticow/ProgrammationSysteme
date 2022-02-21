using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class PauseBackupCommand : DelegateCommandBase
    {
        private InfoBackupViewModel infoBackupViewModel;
        private MenuViewModel menuViewModel;

        public PauseBackupCommand(InfoBackupViewModel viewModel)
        {
            infoBackupViewModel = viewModel;
        }

        public PauseBackupCommand(MenuViewModel viewModel)
        {
            menuViewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            if (infoBackupViewModel != null)
            {
                return infoBackupViewModel.BackupWorkSelected.State == Model.BackupStateEnum.ACTIVE;
            }
            else if (menuViewModel != null)
            {
                return menuViewModel.Selected != null && menuViewModel.Selected.BackupWork.State == Model.BackupStateEnum.ACTIVE;
            }
            return false;
        }

        protected override void Execute(object parameter)
        {
            if (infoBackupViewModel != null)
            {
                infoBackupViewModel.BackupWorkSelected.Pause();
            }
            else if (menuViewModel != null)
            {
                menuViewModel.Selected.BackupWork.Pause();
            }
        }
    }
}
