using Prism.Commands;
using RemoteCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveRemote.ViewModel.Commands
{
    class PlayCommand : DelegateCommandBase
    {
        private ViewModel viewModel;

        public PlayCommand(ViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        protected override bool CanExecute(object parameter)
        {
            return viewModel.SBackupWork != null && viewModel.SBackupWork.Play;
        }

        protected override void Execute(object parameter)
        {
            viewModel.Client.Send(new CommandPacket() { Id = viewModel.SBackupWork.Id, Command = "Play" });
        }
    }
}
