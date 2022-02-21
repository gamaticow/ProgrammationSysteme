using EasySave.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EasySave.ViewModel.Commands
{
    class UpdateViewCommand : DelegateCommandBase
    {
        private BaseViewModel viewModel;

        public UpdateViewCommand(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        protected override bool CanExecute(object parameter)
        {
            if (parameter == null)
                return false;
            if(parameter.ToString() == "InfoBackup")
            {
                if (viewModel.GetType() == typeof(MenuViewModel))
                {
                    MenuViewModel menu = (MenuViewModel)viewModel;
                    if(menu.Selected != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        protected override void Execute(object parameter)
        {
            if (parameter.ToString() == "Menu")
            {
                viewModel.SelectedViewModel = new MenuViewModel();
            }
            else if (parameter.ToString() == "Settings")
            {
                viewModel.SelectedViewModel = new SettingsViewModel();
            }
            else if (parameter.ToString() == "CreateBackup")
            {
                viewModel.SelectedViewModel = new CreateBackupViewModel();
            }
            else if (parameter.ToString() == "InfoBackup")
            {
                if(viewModel.GetType() == typeof(MenuViewModel))
                {
                    MenuViewModel menu = (MenuViewModel)viewModel;
                    viewModel.SelectedViewModel = new InfoBackupViewModel(menu.Selected.BackupWork);
                }
            }
        }
    }
}
