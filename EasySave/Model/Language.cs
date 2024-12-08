﻿using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace EasySave.Model
{
    class Language
    {

        public LanguageType languageType { get; private set; }
        private ResourceManager resourceManager;

        public Language(LanguageType languageType)
        {
            this.languageType = languageType;
            if(languageType == LanguageType.ENGLISH)
            {
                resourceManager = en_language.ResourceManager;
            }
            else if(languageType == LanguageType.FRENCH)
            {
                resourceManager = fr_language.ResourceManager;
            }
        }

        public string Translate(string key)
        {
            string result;
            if(resourceManager != null && (result = resourceManager.GetString(key)) != null)
            {
                return result;
            }
            return $"!{key}!";
        }

    }
}
