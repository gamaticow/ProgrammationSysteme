using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Model;
using EasySave.View;

namespace EasySave.Controller
{
    class BackupController : Controller
    {
        private BackupWork backupWork;
        private BackupView view;

        public BackupController(BackupWork backupWork)
        {
            this.backupWork = backupWork;
            view = new BackupView(this, backupWork);
        }

        public void BackupWorkInformation()
        {
            bool exit = false;
            do
            {
                string result = view.RenderBackupWork();
                switch (result)
                {
                    case "1":
                        view.RenderExecution(backupWork.name);
                        backupWork.ExecuteBackup();
                        view.RenderSucess("backup_execution_finished");
                        break;
                    case "2":
                        EditBackupWork();
                        break;
                    case "3":
                        // Delete backup
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        view.RenderError("error_impossible_action");
                        break;
                }
            } while (!exit);
        }

        private void EditBackupWork()
        {
            string name = view.RenderRenameBackupWork();
            if (Program.instance.BackupNameExists(name))
            {
                view.RenderError("error_name_already_used");
            }
            else
            {
                backupWork.name = name;
            }
        }

    }
}
