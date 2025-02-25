﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EasySave.Model;

namespace EasySave.ViewModel.Commands
{
    class ParallelExecutionCommand : ICommand
    {
        private MenuViewModel viewModel;

        public ParallelExecutionCommand(MenuViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return Model.Model.Instance.backupWorks.Count > 0;
        }

        public void Execute(object parameter)
        {
            foreach (BackupWork backupWork in Model.Model.Instance.backupWorks)
            {
                backupWork.ExecuteBackup();
            }
        }
    }
}
