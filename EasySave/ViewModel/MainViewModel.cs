using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance { get; private set; } = new MainViewModel();

        private string _tflag_name;
        public string Tflag_name
        {
            get
            {
                return _tflag_name;
            }
            set
            {
                _tflag_name = value;
                OnPropertyChanged(nameof(Tflag_name));
            }
        }

        private MainViewModel() { }

        public override void SetTranslation()
        {
            Tflag_name = Translate("flag_name");
        }
    }
}
