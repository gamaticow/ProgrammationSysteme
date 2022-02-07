using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModel.Commands
{
    class DeleteEncryptedExtensionCommand : DelegateCommandBase
    {
        private SettingsViewModel viewModel;

        public DeleteEncryptedExtensionCommand(SettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            return viewModel.SEncryptedExtension != null && viewModel.SEncryptedExtension.Length > 0;
        }

        protected override void Execute(object parameter)
        {
            Model.Model.Instance.encryptedExtensions.Remove(viewModel.SEncryptedExtension);
            viewModel.OnPropertyChanged(nameof(viewModel.EncryptedExtensions));
        }

    }
}
