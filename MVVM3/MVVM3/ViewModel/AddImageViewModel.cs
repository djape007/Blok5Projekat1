using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM3.ViewModel {
    public class AddImageViewModel : BindableBase {

        public MyICommand LoadPhotoCmd { get; private set; }
        public MyICommand AddPhotoCmd { get; private set; }
        //-------------------------------------------------------------
        private string _message;
        private string _title;
        private string _description;
        private string _imgPath;
        #region Props
        public string Message {
            get { return _message; }
            set {
                _message = value;
                OnPropertyChanged("Message");
            }
        }
        public string Title {
            get { return _title; }
            set {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        public string Description {
            get { return _description; }
            set {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public string ImagePath {
            get { return _imgPath; }
            set {
                _imgPath = value;
                OnPropertyChanged("ImagePath");
            }
        }
        #endregion Props

        public AddImageViewModel() {
            Message = "";
            Title = "";
            Description = "";
            ImagePath = Config.dbImgsFolder + Config.addImagePlaceHolder;
            LoadPhotoCmd = new MyICommand(LoadPhoto);
            AddPhotoCmd = new MyICommand(AddPhoto);
        }

        public void LoadPhoto() {
            try {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

                dialog.Title = "Open Image";
                dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                
                Nullable<bool> result = dialog.ShowDialog();

                if (result == true) {
                    string filename = dialog.FileName;
                    ImagePath = filename;
                }
            } catch (Exception e) {
                Message = "Error: " + e.ToString();
                ImagePath = Config.dbImgsFolder + Config.addImagePlaceHolder;
            }
        }

        public void AddPhoto() {
            string emptyImagePath = Config.dbImgsFolder + Config.addImagePlaceHolder;
            

            if (ImagePath == emptyImagePath) {
                Message = "Putanja do slike nije dobra";
                return;
            }
            if (Title.Length == 0) {
                Message = "Naslov ne moze biti prazan";
                return;
            }

            string userImgsPath = Config.dbImgsFolder + Model.User.ActiveUser.Username + "\\";
            string imgFileName = ImagePath.Split('\\').Last();

            if (!Directory.Exists(userImgsPath)) {
                Directory.CreateDirectory(userImgsPath);
            }

            File.Copy(ImagePath, userImgsPath + imgFileName);
            
            Model.Image tmpImage = new Model.Image(Title, Description, Model.User.ActiveUser.Username, imgFileName);
            Database.Instance().AddImage(tmpImage);
        }
    }
}
