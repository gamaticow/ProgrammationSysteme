﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCommon
{
    [Serializable]
    public class AddBackupPacket
    {
        public RemoteBackupWork BackupWork { get; set; }
    }
}
