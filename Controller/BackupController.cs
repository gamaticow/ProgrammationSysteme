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
            view.RenderBackupWork();
        }

        private void EditBackupWork()
        {
            view.RenderRenameBackupWork();
        }

    }
}
