using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public string _tMenu;
        public string TMenu
        {
            get
            {
                return _tMenu;
            }

            set
            {
                _tMenu = value;
                OnPropertyChanged(nameof(TMenu));
            }
        }

        public static MainViewModel Instance { get; private set; } = new MainViewModel();

        private MainViewModel() { }

        public override void SetTranslation()
        {
            TMenu = Translate("menu");
        }
    }
}
