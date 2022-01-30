using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Controller;
using EasySave.Model;

namespace EasySave.View
{
    class MainView : View
    {

        public MainView(MainController controller) : base(controller)
        {}

        public string RenderMain()
        {
            Console.WriteLine($"1. {controller.Translate("mainview_backupworks")}");
            Console.WriteLine($"2. {controller.Translate("mainview_language_configuration")}");
            Console.WriteLine($"3. {controller.Translate("mainview_quit")}");
            RenderChoseAction();
            return Console.ReadLine();
        }

        public string RenderBackupWorkList(BackupWork[] backupWorks)
        {
            int index = 1;
            foreach(BackupWork backupWork in backupWorks)
            {
                Console.WriteLine($"{index++}. {backupWork.name}");
            }
            if(backupWorks.Length > 0)
            {
                Console.WriteLine($"{index++}. {controller.Translate("mainview_execute_all_backup_works")}");
            }
            if(backupWorks.Length < 5)
            {
                Console.WriteLine($"{index++}. {controller.Translate("mainview_create_backup_work")}");
            }
            Console.WriteLine($"{index++}. {controller.Translate("mainview_back")}");
            RenderChoseAction();
            return Console.ReadLine();
        }

        public string RenderLanguageConfiguration()
        {
            Console.WriteLine($"1. {controller.Translate("mainview_language_en")}");
            Console.WriteLine($"2. {controller.Translate("mainview_language_fr")}");
            Console.WriteLine($"3. {controller.Translate("mainview_back")}");
            RenderChoseAction();
            return Console.ReadLine();
        }

        public void RenderLanguageChanged()
        {
            Console.WriteLine(controller.Translate("mainview_language_changed"));
        }

        public string[] RenderCreateBackupWork()
        {
            string[] result = new string[4];
            Console.Write(controller.Translate("mainview_create_backup_name"));
            result[0] = Console.ReadLine();
            Console.Write($"{controller.Translate("mainview_create_backup_source")}");
            result[1] = Console.ReadLine();
            Console.Write($"{controller.Translate("mainview_create_backup_target")}");
            result[2] = Console.ReadLine();
            Console.Write($"{controller.Translate("mainview_create_backup_type")}");
            result[3] = Console.ReadLine();
            return result;
        }

    }
}
