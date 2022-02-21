using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class AddPriorityFileCommand : DelegateCommandBase
    {
        private SettingsViewModel viewModel;

        public AddPriorityFileCommand(SettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            return viewModel.PriorityFileTextBox != null && viewModel.PriorityFileTextBox.Length > 0;
        }

        protected override void Execute(object parameter)
        {
            string priorityFile = viewModel.PriorityFileTextBox;

            if(!Model.Model.Instance.priorityFiles.Contains(priorityFile.ToLower()))
            {
                Model.Model.Instance.priorityFiles.Add(priorityFile.ToLower());
                Model.Model.Instance.WriteDataFile();
                viewModel.OnPropertyChanged(nameof(viewModel.PriorityFiles));
            }

            viewModel.EncryptedExtensionTextBox = "";
            viewModel.OnPropertyChanged(nameof(viewModel.PriorityFileTextBox));
        }
    }
}
