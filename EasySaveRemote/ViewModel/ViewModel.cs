using EasySaveRemote.Model;
using EasySaveRemote.ViewModel.Commands;
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
        public SocketClient Client { get; private set; }

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

        private string _tParallelExecution;
        public string TParallelExecution
        {
            get
            {
                return _tParallelExecution;
            }
            set
            {
                _tParallelExecution = value;
                OnPropertyChanged(nameof(TParallelExecution));
            }
        }

        // Properties
        private int SId;
        private RemoteBackupWork _sBackupWork;
        public RemoteBackupWork SBackupWork
        {
            get
            {
                return _sBackupWork;
            }
            set
            {
                _sBackupWork = value;
                if(value != null)
                    SId = value.Id;
                PlayCommand.RaiseCanExecuteChanged();
                PauseCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<RemoteBackupWork> BackupWorks
        {
            get
            {
                if(Model.Model.Instance.BackupWorks.ContainsKey(SId))
                    SBackupWork = Model.Model.Instance.BackupWorks[SId];
                OnPropertyChanged(nameof(SBackupWork));
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
                Client.Send(new LanguagePacket() { Language = language });
            }
        }
        public ObservableCollection<object> Languages { get; set; }

        // Commands
        public PlayCommand PlayCommand { get; private set; }
        public PauseCommand PauseCommand { get; private set; }
        public StopCommand StopCommand { get; private set; }
        public ParallelCommand ParallelCommand { get; private set; }

        public ViewModel(SocketClient client)
        {
            PlayCommand = new PlayCommand(this);
            PauseCommand = new PauseCommand(this);
            StopCommand = new StopCommand(this);
            ParallelCommand = new ParallelCommand(this);

            this.Client = client;
            client.Update = new Update(Update);
        }

        private void SetTranslations()
        {
            TSelectedBackupWork = Translate("selected_backup_work");
            TParallelExecution = Translate("parallel_execution");
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
