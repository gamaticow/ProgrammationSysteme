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
                    else if (output == backupWorks.Length + 1)
                    {
                        // Execute all backups
                    }
                    else if (output == backupWorks.Length + 2)
                    {
                        view.RenderCreateBackupWork();
                    }
                    else if (output == backupWorks.Length + 3)
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

        private void CreateBackupWork()
        {
            view.RenderCreateBackupWork();
        }

        private void ExecuteSequentialBackup()
        {

        }

    }
}
