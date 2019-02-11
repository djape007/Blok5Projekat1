using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MVVM3 {
    public class Config {
        public static readonly string exeFolderPath = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string addImagePlaceHolder = "AddImage.png";

        public static readonly string dbUsers = @"data\users.xml";
        public static readonly string dbImgs = @"data\imgs.xml";
        public static readonly string dbImgsFolder = exeFolderPath + @"data\images\";

        public static readonly Color defaultNavColor = Colors.CadetBlue;
        public static readonly Color selectedNavColor = Colors.PaleVioletRed;

        public static readonly string defaultTabDisplayed = "myimages";

        public static readonly int minPasswordLength = 6;
    }
}
