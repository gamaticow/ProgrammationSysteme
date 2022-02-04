using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasySave.Model;

namespace EasySave.ViewModel
{
    class LanguageViewModel : BaseViewModel
    {
        private string _tLanguageChoice;
        public string TLanguageChoice
        {
            get
            {
                return _tLanguageChoice;
            }
            set
            {
                _tLanguageChoice = value;
                OnPropertyChanged(nameof(TLanguageChoice));
            }
        }

        private object _sLanguage;
        public object SLanguage
        {
            get
            {
                return _sLanguage;
            }
            set
            {
                _sLanguage = value;
                Type t = value.GetType();
                PropertyInfo info = t.GetProperty("Type");
                LanguageType languageType = (LanguageType) info.GetValue(value);
                Model.Model.Instance.SetLanguage(languageType);
                MainViewModel.Instance.SetTranslation();
                SetTranslation();
            }
        }
        public ObservableCollection<object> Languages { get; set; }

        public LanguageViewModel()
        {
            Languages = new ObservableCollection<object>();
            foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
            {
                var l = new { Name = Translate(language.ToString()), Type = language };
                Languages.Add(l);
                if(language == Model.Model.Instance.language.languageType)
                {
                    SLanguage = l;
                }
            }
        }

        public override void SetTranslation()
        {
            TLanguageChoice = Translate("language_choice");
        }
    }
}
