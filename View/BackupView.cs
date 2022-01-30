using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Model;
using EasySave.Controller;

namespace EasySave.View
{
    class BackupView : View
    {

        private BackupWork backupWork;

        public BackupView(BackupController controller, BackupWork backupWork) : base(controller)
        {
            this.backupWork = backupWork;
        }

        public string RenderBackupWork()
        {
            Console.WriteLine($"{controller.Translate("backupview_selected_backup_name")}{backupWork.name}");
            Console.WriteLine($"{controller.Translate("backupview_selected_backup_source")}{backupWork.sourceDirectory}");
            Console.WriteLine($"{controller.Translate("backupview_selected_backup_target")}{backupWork.targetDirectory}");
            Console.WriteLine($"{controller.Translate("backupview_selected_backup_type")}{controller.Translate(backupWork.backupType.ToString())}");
            Console.WriteLine($"1. {controller.Translate("backupview_execute_backup")}");
            Console.WriteLine($"2. {controller.Translate("backupview_edit_backup")}");
            Console.WriteLine($"3. {controller.Translate("backupview_delete_backup")}");
            Console.WriteLine($"4. {controller.Translate("backupview_back")}");
            RenderChoseAction();
            return Console.ReadLine();
        }

        public string RenderRenameBackupWork()
        {
            Console.Write(controller.Translate("backupview_rename"));
            return Console.ReadLine();
        }

    }
}
