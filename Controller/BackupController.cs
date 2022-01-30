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

        // Controller for backup work information
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
                        if(backupWork.ExecuteBackup())
                        {
                            view.RenderSucess("backup_execution_finished");
                        }
                        else
                        {
                            view.RenderError("error_backup");
                        }
                        break;
                    case "2":
                        EditBackupWork();
                        break;
                    case "3":
                        Program.instance.DeleteBackup(backupWork);
                        view.RenderError("backup_deleted");
                        exit = true;
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

        // Controller for rename backup work
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
