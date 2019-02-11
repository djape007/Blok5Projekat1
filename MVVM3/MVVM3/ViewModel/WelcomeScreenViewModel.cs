using MVVM3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MVVM3.ViewModel {
    public class WelcomeScreenViewModel : BindableBase {
        
        private User _currentUser;

        public User CurrentUser {
            get => _currentUser;
            set {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        public MyICommand<object> LoginCommand { get; set; }
        public MyICommand<object> RegisterCommand { get; set; }

        public WelcomeScreenViewModel () {
            CurrentUser = new User();
            LoginCommand = new MyICommand<object>(OnLogin);
            RegisterCommand = new MyICommand<object>(OnRegister);
        }

        private void SetCurrentUserPassword(object passwordbox) {
            try {
                var passwordBox = (PasswordBox)passwordbox;
                var value = passwordBox.Password;
                CurrentUser.Password = value;
            } catch {
                CurrentUser.Password = "";
            }
        }

        public void OnLogin(object param) {
            SetCurrentUserPassword(param);
            CurrentUser.Validate();
            if (CurrentUser.IsValid) {
                if (CurrentUser.ExistsUserWithSameUsername()) {
                    if (CurrentUser.CheckPassword(Database.Instance().GetUser(CurrentUser.Username).Password)) {
                        Model.User.ActiveUser = CurrentUser;
                        MainWindowViewModel.Instance.OnNav("maincontent");
                    }
                } else {
                    CurrentUser.DisplayErrorUsername("Korisnik ne postoji");
                }
            }
        }

        public void OnRegister(object param) {
            Model.Image i1 = new Model.Image("Naslov", "", "djape55", "slika1.png");
            Model.Image i2 = new Model.Image("Naslov 43434", "ova ima opis", "djape55", "slika2.png");
            Model.Image i3 = new Model.Image("Naslov 56765", "jako sam kul, jeaaa", "djape55", "slika3.jpg");

            Database.Instance().AddImage(i1);
            Database.Instance().AddImage(i2);
            Database.Instance().AddImage(i3);

            SetCurrentUserPassword(param);

            CurrentUser.Validate();
            if (CurrentUser.IsValid) {
                if (CurrentUser.ExistsUserWithSameUsername()) {
                    CurrentUser.DisplayErrorUsername("Korisničko ime već postoji");
                } else {
                    Database.Instance().AddUser(CurrentUser);
                    Model.User.ActiveUser = CurrentUser;
                    MainWindowViewModel.Instance.OnNav("maincontent.addimage");
                }
            }
        }
    }
}
