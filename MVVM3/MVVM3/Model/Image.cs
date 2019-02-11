using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM3.Model {
    public class Image : ValidationBase {
        private static string _defaultPath = Config.dbImgsFolder;

        private string _title;
        private string _description;
        private DateTime _dateAdded;
        private string _name;
        private string _ownerUsername;

        public string Title { get => _title; set => _title = value; }
        public string Description { get => _description; set => _description = value; }
        public DateTime DateAdded { get => _dateAdded; set => _dateAdded = value; }
        public string OwnerUsername { get => _ownerUsername; set => _ownerUsername = value; }
        public string FileName { get => _name; set => _name = value; }
        public string Path {
            get => (_defaultPath + _ownerUsername + "\\" + FileName);
        }

        public Image() {

        }

        public Image(string title, string desc, string ownerUsername, string fileName) {
            Title = title;
            Description = desc;
            OwnerUsername = ownerUsername;
            FileName = fileName;
            OwnerUsername = ownerUsername;
            DateAdded = DateTime.Now;
        }

        protected override void ValidateSelf() {
            if (string.IsNullOrWhiteSpace(this._title)) {
                this.ValidationErrors["Title"] = "Moraš uneti naslov slike";
            }
            /*if (string.IsNullOrWhiteSpace(this._description)) {
                this.ValidationErrors["Description"] = "Moraš uneti opis slike";
            }*/
            if (string.IsNullOrWhiteSpace(this._ownerUsername)) {
                this.ValidationErrors["OwnerUsername"] = "Slika nema vlasnika";
            }
            if (string.IsNullOrWhiteSpace(this._name)) {
                this.ValidationErrors["FileName"] = "Fajl nema naziv";
            }
        }
    }
}
