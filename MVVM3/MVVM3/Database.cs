using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MVVM3.Model;

namespace MVVM3 {
    public class Database : IDataAccess {
        static string dbUsers = Config.dbUsers;
        static string dbImgs = Config.dbImgs;
        static private IDataAccess _instance = null;

        static Dictionary<string, User> allUsers;
        static Dictionary<string, List<Image>> allImages;

        static Database() {
            allUsers = LoadUsersFromFile();
            allImages = LoadImagesFromFile();
        }

        private Database() {
            
        }

        public static IDataAccess Instance() {
            if (_instance == null) {
                _instance = new Database();
            }

            return _instance;
        }

        public void AddImage(Image i) {
            i.Validate();
            if (i.IsValid) {
                if (! allImages.ContainsKey(i.OwnerUsername)) {
                    allImages.Add(i.OwnerUsername, new List<Image>());
                } 

                allImages[i.OwnerUsername].Add(i);

                SaveImagesToFile();
            }
        }

        public void AddUser(User u) {
            u.Validate();
            if (u.IsValid) {
                if (! allUsers.ContainsKey(u.Username)) {
                    allUsers.Add(u.Username, u);
                    SaveUsersToFile();
                }
            }
        }

        public User GetUser(string username) {
            if (allUsers.ContainsKey(username)) {
                return allUsers[username];
            }
            return null;
        }

        public bool UserExists(string username) {
            if (allUsers.ContainsKey(username)) {
                return true;
            }
            return false;
        }

        private static void SaveUsersToFile() {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<User>));
            string xml = "";

            using (var sww = new StringWriter()) {
                using (XmlWriter writer = XmlWriter.Create(sww)) {
                    xsSubmit.Serialize(writer, allUsers.Values.ToList());
                    xml = sww.ToString();
                }
            }

            using (StreamWriter sw = new StreamWriter(dbUsers, false, Encoding.UTF8)) {
                sw.Write(xml);
            }
        }

        private static Dictionary<string, User> LoadUsersFromFile() {
            List<User> allElements = null;
            XmlSerializer deserializer = new XmlSerializer(typeof(List<User>));

            try {
                using (StreamReader sr = new StreamReader(dbUsers, Encoding.UTF8)) {
                    allElements = (List<User>)deserializer.Deserialize(sr);
                }
            } catch {
                return new Dictionary<string, User>();
            }

            if (allElements == null) {
                return new Dictionary<string, User>();
            }

            Dictionary<string, User> returnVal = new Dictionary<string, User>();
            foreach (User korisnik in allElements) {
                if (! returnVal.ContainsKey(korisnik.Username)) {
                    returnVal.Add(korisnik.Username, korisnik);
                }
            }

            return returnVal;
        }

        private static void SaveImagesToFile() {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<List<Image>>));
            string xml = "";

            using (var sww = new StringWriter()) {
                using (XmlWriter writer = XmlWriter.Create(sww)) {
                    xsSubmit.Serialize(writer, allImages.Values.ToList());
                    xml = sww.ToString();
                }
            }

            using (StreamWriter sw = new StreamWriter(dbImgs, false, Encoding.UTF8)) {
                sw.Write(xml);
            }
        }

        private static Dictionary<string, List<Image>> LoadImagesFromFile() {
            List<List<Image>> allElements = null;
            XmlSerializer deserializer = new XmlSerializer(typeof(List<List<Image>>));

            try {
                using (StreamReader sr = new StreamReader(dbImgs)) {
                    allElements = (List<List<Image>>)deserializer.Deserialize(sr);
                }
            } catch {
                return new Dictionary<string, List<Image>>();
            }

            if (allElements == null) {
                return new Dictionary<string, List<Image>>();
            }

            Dictionary<string, List<Image>> returnVal = new Dictionary<string, List<Image>>();
            foreach (List<Image> listaSlika in allElements) {
                if (listaSlika.Count > 0) {
                    returnVal.Add(listaSlika[0].OwnerUsername, listaSlika);
                }
            }

            return returnVal;
        }

        public static MemoryStream GenerateStreamFromString(string value) {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
        
        public void RemoveUser(User u) {
            if (allUsers.ContainsKey(u.Username)) {
                allUsers.Remove(u.Username);
                SaveUsersToFile();
            }
        }

        public bool UpdateUser(User old, User newU) {
            if (!newU.ExistsUserWithSameUsername()) {
                RemoveUser(old);
                AddUser(newU);

                if (allImages.ContainsKey(old.Username)) {
                    var usersImages = allImages[old.Username];

                    foreach(Image tmpI in usersImages) {
                        tmpI.OwnerUsername = newU.Username;
                    }

                    allImages.Remove(old.Username);
                    allImages.Add(newU.Username, usersImages);
                    SaveImagesToFile();
                }

                return true;
            } else {
                return false;
            }
        }

        public List<Image> GetImagesOfUser(string username) {
            if (allImages.ContainsKey(username)) {
                return allImages[username];
            }
            return new List<Image>();
        }
    }
}
