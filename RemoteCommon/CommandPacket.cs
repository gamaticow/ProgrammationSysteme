using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCommon
{
    [Serializable]
    public class CommandPacket
    {
        public int Id { get; set; }
        public string Command { get; set; }
    }
}
