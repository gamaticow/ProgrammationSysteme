using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCommon
{
    [Serializable]
    public class RemoteBackupWork
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Progress { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public bool Play { get; set; }
        public bool Pause { get; set; }
        public bool Stop { get; set; }
    }
}
