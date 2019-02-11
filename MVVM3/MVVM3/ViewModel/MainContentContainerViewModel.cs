using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MVVM3.ViewModel {
    public class MainContentContainerViewModel : BindableBase {

        public static string defaultNavigation = Config.defaultTabDisplayed;
        public static Model.User ActiveUser {
            get => _activeUser;
            set {
                var tmpUser = Database.Instance().GetUser(value.Username);
                _activeUser = ActiveUser;
            }
        }

        private static Model.User _activeUser = null;
        private AddImageViewModel _addImageViewModel = new AddImageViewModel();
        private MyImagesViewModel _myImagesViewModel = new MyImagesViewModel();
        private AccountViewModel _accountViewModel = new AccountViewModel();
        private BindableBase currentViewModel;
        private Brush _myImagesBackground;
        private Brush _addImageBackground;
        private Brush _accountBackground;
        private Color _defaultNavColor = Config.defaultNavColor;
        private Color _selectedNavColor = Config.selectedNavColor;

        public MyICommand<string> NavCommand { get; private set; }
        #region Props
        public Brush MyImagesBackground {
            get => _myImagesBackground;
            set {
                _myImagesBackground = value;
                OnPropertyChanged("MyImagesBackground");
            }
        }
        public Brush AddImageBackground {
            get => _addImageBackground;
            set {
                _addImageBackground = value;
                OnPropertyChanged("AddImageBackground");
            }
        }
        public Brush AccountBackground {
            get => _accountBackground;
            set {
                _accountBackground = value;
                OnPropertyChanged("AccountBackground");
            }
        }

        public BindableBase CurrentViewModel {
            get { return currentViewModel; }
            set {
                SetProperty(ref currentViewModel, value);
            }
        }
        #endregion Props

        public MainContentContainerViewModel() {
            NavCommand = new MyICommand<string>(OnNav);
            OnNav(defaultNavigation);
        }

        public void OnNav(string destination) {
            MyImagesBackground = new SolidColorBrush(_defaultNavColor);
            AddImageBackground = new SolidColorBrush(_defaultNavColor);
            AccountBackground = new SolidColorBrush(_defaultNavColor);

            switch (destination) {
                case "myimages":
                    CurrentViewModel = _myImagesViewModel;
                    MyImagesBackground = new SolidColorBrush(_selectedNavColor);
                break;
                case "addimage":
                    CurrentViewModel = _addImageViewModel;
                    AddImageBackground = new SolidColorBrush(_selectedNavColor);
                break;
                case "account":
                    CurrentViewModel = _accountViewModel;
                    AccountBackground = new SolidColorBrush(_selectedNavColor);
                break;
            }
        }
    }
}
