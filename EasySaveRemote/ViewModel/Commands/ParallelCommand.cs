using Prism.Commands;
using RemoteCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveRemote.ViewModel.Commands
{
    class ParallelCommand : DelegateCommandBase
    {
        private ViewModel viewModel;

        public ParallelCommand(ViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            return viewModel.BackupWorks.Count > 0;
        }

        protected override void Execute(object parameter)
        {
            viewModel.Client.Send(new CommandPacket() { Command = "Parallel" });
        }
    }
}
