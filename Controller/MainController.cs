using System;
using System.Collections.Generic;
using System.Text;
using EasySave.View;
using EasySave.Model;

namespace EasySave.Controller
{
    class MainController : Controller
    {

        private MainView view;

        public MainController()
        {
            this.view = new MainView(this);
        }

        // Controller fo main menu
        public void Main()
        {
            bool exit = false;
            do
            {
                string result = view.RenderMain();
                switch (result)
                {
                    case "1":
                        BackupWorkList();
                        break;
                    case "2":
                        LanguageConfiguration();
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        view.RenderError("error_impossible_action");
                        break;
                }
            } while (!exit);
        }

        // Controller for backup list
        private void BackupWorkList()
        {
            bool exit = false;
            do
            {
                BackupWork[] backupWorks = this.backupWorks;
                string result = view.RenderBackupWorkList(backupWorks);
                try
                {
                    int output = Int32.Parse(result);
                    if (output <= backupWorks.Length)
                    {
                        Program.instance.OpenBackupController(backupWorks[output - 1]);
                    }
                    // If the number of backup works >= 1 : User can execute backup
                    else if (backupWorks.Length > 0 && output == backupWorks.Length + 1)
                    {
                        ExecuteSequentialBackup();
                    }
                    // If number of backup works < 5 : User can create backupwork
                    else if (backupWorks.Length < 5 && output == backupWorks.Length + (backupWorks.Length > 0 ? 2 : 1))
                    {
                        CreateBackupWork();
                    }
                    else if (output == backupWorks.Length + (backupWorks.Length > 0 && backupWorks.Length < 5 ? 3 : 2))
                    {
                        exit = true;
                    } 
                    else
                    {
                        view.RenderError("error_impossible_action");
                    }
                }
                catch (FormatException e)
                {
                    view.RenderError("error_impossible_action");
                }
            } while (!exit);
        }

        // Controller for language configuration
/*
        private void LanguageConfiguration()
        {
            bool exit = true;
            do
            {
                string result = view.RenderLanguageConfiguration();
                switch (result)
                {
                    case "1":
                        Program.instance.SetLanguage(Model.LanguageType.ENGLISH);
                        view.RenderLanguageChanged();
                        break;
                    case "2":
                        Program.instance.SetLanguage(Model.LanguageType.FRENCH);
                        view.RenderLanguageChanged();
                        break;
                    case "3":
                        break;
                    default:
                        view.RenderError("error_impossible_action");
                        exit = false;
                        break;
                }
            } while (!exit);
        }
*/

        // Controller for Log file type configuration
        private void LogTypeConfiguration()
        {
            bool exit = true;
            do
            {
                string result = view.RenderLogTypeConfiguration();
                switch (result)
                {
                    case "1":
                        Program.instance.SetLanguage(Model.LanguageType.ENGLISH);
                        view.RenderLanguageChanged();
                        break;
                    case "2":
                        Program.instance.SetLanguage(Model.LanguageType.FRENCH);
                        view.RenderLanguageChanged();
                        break;
                    case "3":
                        break;
                    default:
                        view.RenderError("error_impossible_action");
                        exit = false;
                        break;
                }
            } while (!exit);
        }

        // Controller for create backup work
        private void CreateBackupWork()
        {
            string[] output = view.RenderCreateBackupWork();
            int result = Program.instance.CreateBackupWork(output[0], output[1], output[2], output[3]);
            switch(result)
            {
                case 0:
                    view.RenderSucess("backupview_backup_created");
                    break;
                case 1:
                    view.RenderError("error_unknown_type");
                    break;
                case 2:
                    view.RenderError("error_name_already_used");
                    break;
                case 3:
                    view.RenderError("error_fields_empty");
                    break;
            }
        }

        // Execute all backup works
        private void ExecuteSequentialBackup()
        {
            foreach (BackupWork backupWork in backupWorks)
            {
                view.RenderExecution(backupWork.name);
                if (backupWork.ExecuteBackup())
                {
                    view.RenderSucess("backup_execution_finished");
                }
                else
                {
                    view.RenderError("error_backup");
                }
            }
        }

    }
}
