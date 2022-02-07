using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasySave.Model;
using EasySave.ViewModel.Commands;

namespace EasySave.ViewModel
{
    class SettingsViewModel : BaseViewModel
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

        private string _tEncryptedExtension;
        public string TEncryptedExtension
        {
            get
            {
                return _tEncryptedExtension;
            }
            set
            {
                _tEncryptedExtension = value;
                OnPropertyChanged(nameof(TEncryptedExtension));
            }
        }

        private string _tEncryptedExtensionAdd;
        public string TEncryptedExtensionAdd
        {
            get
            {
                return _tEncryptedExtensionAdd;
            }
            set
            {
                _tEncryptedExtensionAdd = value;
                OnPropertyChanged(nameof(TEncryptedExtensionAdd));
            }
        }

        private string _tEncryptedExtensionDelete;
        public string TEncryptedExtensionDelete
        {
            get
            {
                return _tEncryptedExtensionDelete;
            }
            set
            {
                _tEncryptedExtensionDelete = value;
                OnPropertyChanged(nameof(TEncryptedExtensionDelete));
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

        private string _sEncryptedExtension;
        public string SEncryptedExtension
        {
            get
            {
                return _sEncryptedExtension;
            }
            set
            {
                _sEncryptedExtension = value;
                DeleteEncryptedExtensionCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<string> EncryptedExtensions
        {
            get
            {
                return new ObservableCollection<string>(Model.Model.Instance.encryptedExtensions);
            }
            set
            {

            }
        }

        private string _encryptedExtensionTextBox;
        public string EncryptedExtensionTextBox
        {
            get
            {
                return _encryptedExtensionTextBox;
            }
            set
            {
                _encryptedExtensionTextBox = value;
                AddEncryptedExtensionCommand.RaiseCanExecuteChanged();
            }
        }

        public AddEncryptedExtensionCommand AddEncryptedExtensionCommand { get; private set; }

        public DeleteEncryptedExtensionCommand DeleteEncryptedExtensionCommand { get; private set; }

        public SettingsViewModel()
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

            AddEncryptedExtensionCommand = new AddEncryptedExtensionCommand(this);
            DeleteEncryptedExtensionCommand = new DeleteEncryptedExtensionCommand(this);
        }

        public override void SetTranslation()
        {
            TLanguageChoice = Translate("language_choice");
            TEncryptedExtension = Translate("encrypted_extension");
            TEncryptedExtensionAdd = Translate("encrypted_extension_add");
            TEncryptedExtensionDelete = Translate("encrypted_extension_delete");
        }
    }
}
