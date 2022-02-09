using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EasySave.ViewModel.Commands
{
    class MediaPlayerCommand: ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (Model.Model.Instance.mediaPlayer != null)
            {
                Model.Model.Instance.mediaPlayer.Stop();
            }
            Type t = Model.Model.Instance.Music.GetType();
            PropertyInfo info = t.GetProperty("Path");
            Uri uri = new Uri((string)info.GetValue(Model.Model.Instance.Music));
            Model.Model.Instance.mediaPlayer = new MediaPlayer();
            Model.Model.Instance.mediaPlayer.Open(uri);
            Model.Model.Instance.mediaPlayer.Play();
        }
    }
}
