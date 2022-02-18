using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModel.Commands
{
    class DeletePriorityFileCommand : DelegateCommandBase
    {
        private SettingsViewModel viewModel;

        public DeletePriorityFileCommand(SettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            return viewModel.SPriorityFile != null && viewModel.SPriorityFile.Length > 0;
        }

        protected override void Execute(object parameter)
        {
            Model.Model.Instance.priorityFiles.Remove(viewModel.SPriorityFile);
            Model.Model.Instance.WriteDataFile();
            viewModel.OnPropertyChanged(nameof(viewModel.PriorityFiles));
        }

    }
}
