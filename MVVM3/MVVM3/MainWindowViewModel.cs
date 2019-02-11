using MVVM3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM3.ViewModel;

namespace MVVM3 {
    public class MainWindowViewModel : BindableBase {

        public static MainWindowViewModel Instance { get; private set; }
        public MyICommand<string> NavCommand { get; private set; }
        private MainContentContainerViewModel mainContentViewModel = new MainContentContainerViewModel();
        private WelcomeScreenViewModel welcomeScreenViewmodel = new WelcomeScreenViewModel();
        private BindableBase currentViewModel;

        public MainWindowViewModel() {
            Instance = this;
            NavCommand = new MyICommand<string>(OnNav);
            OnNav("welcome");
        }

        public BindableBase CurrentViewModel {
            get { return currentViewModel; }
            set {
                SetProperty(ref currentViewModel, value);
            }
        }

        public void OnNav(string destination) {
            switch (destination) {
                case "welcome":
                    CurrentViewModel = welcomeScreenViewmodel;
                break;
                case "maincontent.addimage":
                    MainContentContainerViewModel.defaultNavigation = "addimage";
                    CurrentViewModel = mainContentViewModel;
                break;
                case "maincontent":
                    CurrentViewModel = mainContentViewModel;
                break;
                
            }
        }


    }
}
