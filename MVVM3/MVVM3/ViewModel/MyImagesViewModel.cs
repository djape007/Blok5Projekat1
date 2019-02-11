using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM3.ViewModel {
    public class MyImagesViewModel : BindableBase {

        ObservableCollection<Model.Image> _myImages = new ObservableCollection<Model.Image>();

        public ObservableCollection<Model.Image> MyImages {
            get => _myImages; 
            set {
                _myImages = value;
                OnPropertyChanged("MyImages");
            }
        }

        public MyImagesViewModel () {
            if (Model.User.ActiveUser != null) {
                MyImages = new ObservableCollection<Model.Image>(Database.Instance().GetImagesOfUser(Model.User.ActiveUser.Username));
            }
        }

    }
}
