using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class UpdateViewCommand : ICommand
    {
        private BaseViewModel viewModel;

        public UpdateViewCommand(BaseViewModel viewModel)
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
                    viewModel.SelectedViewModel = new InfoBackupViewModel(menu.Selected);
                }
            }
        }
    }
}
