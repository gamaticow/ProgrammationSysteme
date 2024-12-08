using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySave.ViewModel.Commands
{
    class AddEncryptedExtensionCommand : DelegateCommandBase
    {
        private SettingsViewModel viewModel;

        public AddEncryptedExtensionCommand(SettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            return viewModel.EncryptedExtensionTextBox != null && viewModel.EncryptedExtensionTextBox.Length > 0;
        }

        protected override void Execute(object parameter)
        {
            string extension = viewModel.EncryptedExtensionTextBox;
            if(!extension.StartsWith("."))
            {
                extension = $".{extension}";
            }

            if(!Model.Model.Instance.encryptedExtensions.Contains(extension))
            {
                Model.Model.Instance.encryptedExtensions.Add(extension);
                Model.Model.Instance.WriteDataFile();
                viewModel.OnPropertyChanged(nameof(viewModel.EncryptedExtensions));
            }

            viewModel.EncryptedExtensionTextBox = "";
            viewModel.OnPropertyChanged(nameof(viewModel.EncryptedExtensionTextBox));
        }
    }
}
