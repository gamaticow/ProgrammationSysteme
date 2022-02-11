using EasySave.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class SelectFolderCommand : ICommand
    {
        private CreateBackupViewModel viewModel;

        public SelectFolderCommand(CreateBackupViewModel viewModel)
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
            FolderPicker folderPicker = new FolderPicker();
            if(folderPicker.ShowDialog() == true)
            {
                if (parameter.ToString() == "source")
                {
                    viewModel.sourceDirectory = folderPicker.ResultPath;
                }
                else if (parameter.ToString() == "target")
                {
                    viewModel.targetDirectory = folderPicker.ResultPath;
                }
            }
        }
    }
}
