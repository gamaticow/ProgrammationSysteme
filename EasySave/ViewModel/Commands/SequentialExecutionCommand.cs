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
    class SequentialExecutionCommand: ICommand
    {
        private MenuViewModel viewModel;

        public SequentialExecutionCommand(MenuViewModel viewModel)
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
            foreach (BackupWork backupWork in Model.Model.Instance.backupWorks)
            {
                if (backupWork.ExecuteBackup())
                {
                }
                else
                {
                    MessageBox.Show(Model.Model.Instance.language.Translate("error_sequential_execution"), Model.Model.Instance.language.Translate("error_sequential_execution"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
