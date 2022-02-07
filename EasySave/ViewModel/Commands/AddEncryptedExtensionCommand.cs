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
            Model.Model.Instance.encryptedExtensions.Add(viewModel.EncryptedExtensionTextBox);
            viewModel.OnPropertyChanged(nameof(viewModel.EncryptedExtensions));
        }
    }
}
