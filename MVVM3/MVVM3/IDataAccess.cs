using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM3.Model;

namespace MVVM3 {
    public interface IDataAccess {
        void AddUser(User u);
        void RemoveUser(User u);
        bool UpdateUser(User old, User newU);
        bool UserExists(string username);
        User GetUser(string username);
        void AddImage(Image i);
        List<Image> GetImagesOfUser(string username);
    }
}
