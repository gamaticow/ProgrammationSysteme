using System;
using System.Collections.Generic;
using System.Linq;
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
            Uri uri = new Uri(@"pack://siteoforigin:,,,/Resources/bob_marley_1.mp3");
            Model.Model.Instance.mediaPlayer = new MediaPlayer();
            Model.Model.Instance.mediaPlayer.Open(uri);
            Model.Model.Instance.mediaPlayer.Play();
        }
    }
}
