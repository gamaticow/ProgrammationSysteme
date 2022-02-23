using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCommon
{
    [Serializable]
    public class LanguagePacket
    {
        public string Language { get; set; }
        public Dictionary<string, string> Translations { get; set; }
    }
}
