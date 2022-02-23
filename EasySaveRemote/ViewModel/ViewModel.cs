using EasySaveRemote.Model;
using RemoteCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveRemote.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        private SocketClient client;

        public event PropertyChangedEventHandler PropertyChanged;

        // Translations
        private string _tSelectedBackupWork;
        public string TSelectedBackupWork
        {
            get
            {
                return _tSelectedBackupWork;
            }
            set
            {
                _tSelectedBackupWork = value;
                OnPropertyChanged(nameof(TSelectedBackupWork));
            }
        }

        // Properties
        public ObservableCollection<RemoteBackupWork> BackupWorks
        {
            get
            {
                return new ObservableCollection<RemoteBackupWork>(Model.Model.Instance.BackupWorks.Values);
            }
            set
            {

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
                string language = (string)info.GetValue(value);
                client.Send(new LanguagePacket() { Language = language });
            }
        }
        public ObservableCollection<object> Languages { get; set; }

        public ViewModel(SocketClient client)
        {
            this.client = client;
            client.Update = new Update(Update);
        }

        private void SetTranslations()
        {
            TSelectedBackupWork = Translate("selected_backup_work");
        }

        private string Translate(string key)
        {
            return Model.Model.Instance.Language.Translate(key);
        }

        public void Update(string type)
        {
            if(type == "Init")
            {
                Languages = new ObservableCollection<object>();
                foreach (string language in Model.Model.Instance.AvailableLanguages)
                {
                    object obj = new { Type = language, Icon = Translate(language) };
                    Languages.Add(obj);
                    if(language == Model.Model.Instance.Language.Name)
                    {
                        SLanguage = obj;
                    }
                }
                Update("Language");
            }
            else if (type == "Language")
            {
                SetTranslations();
            }
            else if (type == "AddBackupWork" || type == "DeleteBackupWork" || type == "Update")
            {
                OnPropertyChanged(nameof(BackupWorks));
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
