using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class SelectFileCommand : ICommand
    {
        private SettingsViewModel viewModel;

        public SelectFileCommand(SettingsViewModel viewModel)
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
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Executable (*.exe)|*.exe";
            bool? result = openFile.ShowDialog();
            if (result.HasValue && result.Value)
            {
                viewModel.BusinessApp = openFile.FileName;
            }
        }
    }
}
