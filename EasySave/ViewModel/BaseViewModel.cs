using EasySave.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasySave.Model;

namespace EasySave.ViewModel
{
    abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static BaseViewModel _selectedViewModel = new MenuViewModel();
        public BaseViewModel SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                MainViewModel.Instance.OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand UpdateViewCommand { get; set; }

        public BaseViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            SetTranslation();
        }

        public abstract void SetTranslation();

        protected string Translate(string key)
        {
            return Model.Model.Instance.language.Translate(key);
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
