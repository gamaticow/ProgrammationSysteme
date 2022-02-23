using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCommon
{
    [Serializable]
    public class InitPacket
    {
        public List<string> AvailableLanguages { get; set; }
        public List<RemoteBackupWork> RemoteBackupWorks { get; set; }
        public LanguagePacket LanguagePacket { get; set; }
    }
}
