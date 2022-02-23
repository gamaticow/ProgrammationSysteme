using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EasySave.ViewModel;

namespace EasySave
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mutex mutex = new Mutex(true, "{ea88457f-79b4-4d23-bfee-3376cee292f2}");
        bool hasMutex = false;

        public MainWindow()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
<<<<<<< HEAD
                var uri = new Uri(@"pack://siteoforigin:,,,/Resources/windows_xp.mp3", UriKind.RelativeOrAbsolute);
=======
                hasMutex = true;

                var uri = new Uri(@"pack://siteoforigin:,,,/Resources/Contre_sens.mp3", UriKind.RelativeOrAbsolute);
>>>>>>> 77b864ad40649e817dd068587353e1eb04b92966
                Model.Model.Instance.mediaPlayer = new MediaPlayer();
                Model.Model.Instance.mediaPlayer.Open(uri);
                //Model.Model.Instance.mediaPlayer.Play();

                Model.Model.Instance.StartServer();

                InitializeComponent();
                DataContext = MainViewModel.Instance;
            }
            else
            {
                MessageBox.Show(Model.Model.Instance.language.Translate("error_mono_instance"), Model.Model.Instance.language.Translate("error_title"), MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if(hasMutex)
                mutex.ReleaseMutex();
        }
    }
}
