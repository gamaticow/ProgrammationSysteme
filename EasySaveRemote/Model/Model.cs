using RemoteCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveRemote.Model
{
    class Model
    {
        public static Model Instance { get; private set; } = new Model();

        public List<string> AvailableLanguages { get; set; } = new List<string>();
        public Language Language { get; set; }
        public Dictionary<int, RemoteBackupWork> BackupWorks { get; set; } = new Dictionary<int, RemoteBackupWork>();

        private Model()
        {
            Language = new Language("", new Dictionary<string, string>());
        }
    }
}
