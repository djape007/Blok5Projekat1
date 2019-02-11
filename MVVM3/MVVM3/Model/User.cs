using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVVM3.Model {
    public class User : ValidationBase {
        public static User ActiveUser = null;

        private string _username;
        private string _password;

        public string Username {
            get => _username;
            set {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Password {
            get => _password;
            set {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public bool ExistsUserWithSameUsername() {
            if (Database.Instance().UserExists(_username)) {
                return true;
            }
            return false;
        }

        public bool CheckPassword(string passw) {
            if (_password == passw) {
                return true;
            } else {
                DisplayErrorPassword("Šifra nije ispravna");
                return false;
            }
        }

        public User Clone() {
            User newUser = new User() { Username = _username, Password = _password };
            return newUser;
        }

        public void DisplayErrorUsername(string errorMsg) {
            this.ValidationErrors["Username"] = errorMsg;
            this.OnPropertyChanged("IsValid");
            this.OnPropertyChanged("ValidationErrors");
        }

        public void DisplayErrorPassword(string errorMsg) {
            this.ValidationErrors["Password"] = errorMsg;
            this.OnPropertyChanged("IsValid");
            this.OnPropertyChanged("ValidationErrors");
        }

        protected override void ValidateSelf() {
            if (string.IsNullOrWhiteSpace(this._username)) {
                this.ValidationErrors["Username"] = "Moraš uneti korisničko ime";
            } else if (Regex.IsMatch(this._username, @"^\d+")) {
                this.ValidationErrors["Username"] = "Korisničko ime ne može početi brojem";
            }

            if (string.IsNullOrWhiteSpace(this._password)) {
                this.ValidationErrors["Password"] = "Moraš uneti šifru";
            } else if (this._password.Length < Config.minPasswordLength) {
                this.ValidationErrors["Password"] = "Šifra mora imati minimalno "+ Config.minPasswordLength + " znaka";
            }
        }
    }
}
