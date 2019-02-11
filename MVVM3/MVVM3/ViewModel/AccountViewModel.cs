using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MVVM3.ViewModel {
    public class AccountViewModel : BindableBase {

        public MyICommand<object> ChangeAccountDetails { get; private set; }
        private Model.User _currentUser = null;
        private string _success = "";

        public Model.User CurrentUser {
            get => _currentUser;
            set {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        public string Success {
            get => _success;
            set {
                _success = value;
                OnPropertyChanged("Success");
            }
        }

        public AccountViewModel() {
            ChangeAccountDetails = new MyICommand<object>(OnChangeAccount);
            if (Model.User.ActiveUser != null) {
                _currentUser = Model.User.ActiveUser.Clone();
            }
        }

        private void ChangeFolderName(string old, string newName) {

            Directory.Move(Config.dbImgsFolder + old, Config.dbImgsFolder + newName);
        }

        public void OnChangeAccount(object passwBox) {
            Success = "";
            SetCurrentUserPassword(passwBox);
            _currentUser.Validate();
            if (_currentUser.IsValid) {
                if (Database.Instance().UpdateUser(Model.User.ActiveUser, _currentUser)) {
                    ChangeFolderName(Model.User.ActiveUser.Username, _currentUser.Username);
                    Model.User.ActiveUser = _currentUser;
                    Success = "Promene su sačuvane";
                } else {
                    _currentUser.DisplayErrorUsername("Korisničko ime je zauzeto");
                }
            }
        }

        private void SetCurrentUserPassword(object passwordbox) {
            try {
                var passwordBox = (PasswordBox)passwordbox;
                var value = passwordBox.Password;
                _currentUser.Password = value;
            } catch {
                _currentUser.Password = "";
            }
        }
    }
}
