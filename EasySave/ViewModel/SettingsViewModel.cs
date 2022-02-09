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

        private string _tBusinessApp;
        public string TBusinessApp
        {
            get
            {
                return _tBusinessApp;
            }
            set
            {
                _tBusinessApp = value;
                OnPropertyChanged(nameof(TBusinessApp));
            }
        }

        private string _tLogTypeChoice;
        public string TLogTypeChoice
        {
            get
            {
                return _tLogTypeChoice;
            }
            set
            {
                _tLogTypeChoice = value;
                OnPropertyChanged(nameof(TLogTypeChoice));
            }
        }

        private string _tMusicChoice;
        public string TMusicChoice
        {
            get
            {
                return _tMusicChoice;
            }
            set
            {
                _tMusicChoice = value;
                OnPropertyChanged(nameof(TMusicChoice));
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

        public string BusinessApp
        {
            get
            {
                return Model.Model.Instance.businessApp;
            }
            set
            {
                Model.Model.Instance.businessApp = value;
                Model.Model.Instance.WriteDataFile();
                OnPropertyChanged(nameof(BusinessApp));
            }
        }

        public SelectBusinessAppCommand SelectBuisnessAppExeCommand { get; private set; }

        private object _sLogType;
        public object SLogType
        {
            get
            {
                return _sLogType;
            }
            set
            {
                _sLogType = value;
                Type t = value.GetType();
                PropertyInfo info = t.GetProperty("Type");
                LogType logType = (LogType)info.GetValue(value);
                Model.Model.Instance.logType = logType;
                Model.Model.Instance.WriteDataFile();
            }
        }
        public ObservableCollection<object> LogTypes { get; set; }

        private object _sMusic;
        public object SMusic
        {
            get
            {
                return _sMusic;
            }
            set
            {
                _sMusic = value;
                Model.Model.Instance.Music=value;
                Model.Model.Instance.WriteDataFile();
            }
        }
        public ObservableCollection<object> Playlist { get; set; }
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

            LogTypes = new ObservableCollection<object>();
            foreach (LogType type in Enum.GetValues(typeof(LogType)))
            {
                var l = new { Name = type.ToString(), Type = type };
                LogTypes.Add(l);
                if (type == Model.Model.Instance.logType)
                {
                    SLogType = l;
                }
            }

            Playlist = new ObservableCollection<object>();
            object selected = new { Name = "Bob Marley - Love Is Love", Path = @"pack://siteoforigin:,,,/Resources/bob_marley_LoveIsLove.mp3" };
            SMusic = selected;
            Playlist.Add(selected);
            Playlist.Add(new { Name = "Bob Marley - No Woman No Cry", Path = @"pack://siteoforigin:,,,/Resources/bob_marley_NoWomanNoCry.mp3" });
            Playlist.Add(new { Name = "Makassy - Doucement", Path = @"pack://siteoforigin:,,,/Resources/makassy_doucement.mp3" });
            Playlist.Add(new { Name = "Florent Pagny - Savoir Aimer", Path = @"pack://siteoforigin:,,,/Resources/florent_pagny_savoir_aimer.mp3" });
            Playlist.Add(new { Name = "JUL - Alors la zone", Path = @"pack://siteoforigin:,,,/Resources/jul_alors_la_zone.mp3" });
            Playlist.Add(new { Name = "JUL - On m'appelle l'ovni", Path = @"pack://siteoforigin:,,,/Resources/jul_on_mappelle_lovni.mp3" });
            Playlist.Add(new { Name = "Ninja Turtles - Theme", Path = @"pack://siteoforigin:,,,/Resources/ninja_turtles.mp3" });
            Playlist.Add(new { Name = "Colonel Reyel - Celui", Path = @"pack://siteoforigin:,,,/Resources/colonel_reyel_celui.mp3" });
            Playlist.Add(new { Name = "Jack Uzi - Ford Freestyle", Path = @"pack://siteoforigin:,,,/Resources/jack_uzi_ford_freestyle.mp3" });
            AddEncryptedExtensionCommand = new AddEncryptedExtensionCommand(this);
            DeleteEncryptedExtensionCommand = new DeleteEncryptedExtensionCommand(this);
            SelectBuisnessAppExeCommand = new SelectBusinessAppCommand(this);
        }

        public override void SetTranslation()
        {
            TLanguageChoice = Translate("language_choice");
            TEncryptedExtension = Translate("encrypted_extension");
            TEncryptedExtensionAdd = Translate("encrypted_extension_add");
            TEncryptedExtensionDelete = Translate("encrypted_extension_delete");
            TBusinessApp = Translate("select_business_app");
            TLogTypeChoice = Translate("log_type_choice");
            TMusicChoice = Translate("music_choice");
        }
    }
}
