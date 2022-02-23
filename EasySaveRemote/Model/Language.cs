using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveRemote.Model
{
    class Language
    {
        public string Name { get; private set; }
        private Dictionary<string, string> translations;

        public Language(string name, Dictionary<string, string> translations)
        {
            this.Name = name;
            this.translations = translations;
        }

        public string Translate(string key)
        {
            string result;
            if (translations.TryGetValue(key, out result))
            {
                return result;
            }
            return $"!{key}!";
        }
    }
}
