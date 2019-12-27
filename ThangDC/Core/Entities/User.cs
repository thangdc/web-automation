using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using ThangDC.Core.Securities;

namespace ThangDC.Core.Entities
{
    public class User
    {
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _Confirm;

        public string Confirm
        {
            get { return _Confirm; }
            set { _Confirm = value; }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _PrivateKey;
        public string PrivateKey
        {
            get { return _PrivateKey; }
            set { _PrivateKey = value; }
        }

        private string _PublicKey;
        public string PublicKey
        {
            get { return _PublicKey; }
            set { _PublicKey = value; }
        }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        private string _Salt;

        public string Salt
        {
            get { return _Salt; }
            set { _Salt = value; }
        }

        public User()
        {

        }

        private static User _Current;
        public static User Current
        {
            get
            {
                if (_Current != null)
                {
                    return _Current;
                }
                return null;
            }
            set
            {
                _Current = value;
            }
        }

        public int Login()
        {
            int result = 0;

            if (string.IsNullOrEmpty(UserName))
            {
                result = -2;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                result = -3;
            }
            else
            {
                if (File.Exists(Path))
                {
                    var security = new Security(Password);

                    var doc = security.ReadUserConfiguration(Path);

                    string _username = "";
                    string _password = "";
                    string _email = "";
                    string _publickey = "";
                    string _privatekey = "";
                    string _path = "";
                    string _salt = "";

                    foreach (XmlNode node in doc.SelectNodes("root/users/user"))
                    {
                        if (node.SelectSingleNode("username") != null && node.SelectSingleNode("password") != null)
                        {
                            _username = node.SelectSingleNode("username").InnerText;
                            _password = node.SelectSingleNode("password").InnerText;
                            _salt = node.SelectSingleNode("salt").InnerText;

                            if (UserName == _username && security.CheckPassword(Password, Convert.FromBase64String(_salt), Convert.FromBase64String(_password)))
                            {
                                var us = new User();

                                _email = node.SelectSingleNode("email").InnerText;
                                _publickey = node.SelectSingleNode("publickey").InnerText;
                                _privatekey = node.SelectSingleNode("privatekey").InnerText;

                                _path = Path;

                                us.UserName = _username;
                                us.Email = _email;
                                us.PublicKey = _publickey;
                                us.PrivateKey = _privatekey;
                                us.Path = _path;
                                us.Password = Password;

                                _username = null;
                                _password = null;
                                _email = null;
                                _publickey = null;
                                _privatekey = null;
                                

                                User.Current = us;
                                result = 1;
                                break;
                            }
                            else
                            {
                                result = -4;
                            }

                        }
                    }
                }
                else
                {
                    result = -3;
                }
            }

            return result;
        }

        public void Logout()
        {
            User.Current = null;
        }

        public int Register()
        {
            int result = 0;

            if (string.IsNullOrEmpty(UserName))
            {
                result = -1;
            }
            else if (string.IsNullOrEmpty(Email))
            {
                result = -2;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                result = -3;
            }
            else if (string.IsNullOrEmpty(Confirm))
            {
                result = -4;
            }
            else
            {
                string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
                bool checkEmail = System.Text.RegularExpressions.Regex.IsMatch(Email, MatchEmailPattern);
                if (!checkEmail)
                {
                    result = -5;
                }
                else
                {
                    if (Password != Confirm)
                    {
                        result = -6;
                    }
                    else
                    {
                        string publicKey = "";
                        string privateKey = "";

                        CspParameters cspParams = null;
                        RSACryptoServiceProvider rsaProvider = null;

                        cspParams = new CspParameters();
                        cspParams.ProviderType = 1;
                        cspParams.Flags = CspProviderFlags.UseArchivableKey;
                        cspParams.KeyNumber = (int)KeyNumber.Exchange;
                        rsaProvider = new RSACryptoServiceProvider(cspParams);

                        publicKey = rsaProvider.ToXmlString(false);
                        privateKey = rsaProvider.ToXmlString(true);

                        var us = new User();
                        us.UserName = UserName;
                        us.Email = Email;
                        
                        us.PrivateKey = privateKey;
                        us.PublicKey = publicKey;
                        us.Path = Path;

                        byte[] salt = new byte[16];
                        byte[] hashPassword = new byte[16];

                        var security = new Security(Password);
                        security.HashPassword(Password, out salt, out hashPassword);

                        string s = Convert.ToBase64String(salt);
                        string p = Convert.ToBase64String(hashPassword);

                        us.Password = p;
                        us.Salt = s;

                        if (!System.IO.File.Exists(Path))
                        {
                            
                            security.SaveConnectionConfiguration(Path, "test", "test", "test", "test", "test", "test");
                            security.SaveMailConfiguration(Path, "test", "test", 0, "test", "test");
                            security.SaveAccountsConfiguration(Path, "test", "test", "test", "test");
                            security.SaveUserConfiguration(Path, UserName, Email, p, privateKey, publicKey, s);                            

                            Login();

                            result = 1;
                        }
                        else
                        {
                            
                            var users = security.ReadUserConfiguration(Path);
                            
                            bool check = CheckUserAndEmailExists(UserName, Email, users);
                            if (!check)
                            {
                                var node = users.SelectSingleNode("/root/users");
                                var userNode = users.CreateElement("user");

                                node.AppendChild(userNode);

                                var usernameNode = users.CreateElement("username");
                                usernameNode.AppendChild(users.CreateTextNode(UserName));
                                userNode.AppendChild(usernameNode);

                                var emailNode = users.CreateElement("email");
                                emailNode.AppendChild(users.CreateTextNode(Email));
                                userNode.AppendChild(emailNode);

                                var passNode = users.CreateElement("password");
                                passNode.AppendChild(users.CreateTextNode(p));
                                userNode.AppendChild(passNode);

                                var publickeyNode = users.CreateElement("publickey");
                                publickeyNode.AppendChild(users.CreateCDataSection(PublicKey));
                                userNode.AppendChild(publickeyNode);

                                var privateNode = users.CreateElement("privatekey");
                                privateNode.AppendChild(users.CreateCDataSection(PrivateKey));
                                userNode.AppendChild(privateNode);

                                var saltNode = users.CreateElement("salt");
                                saltNode.AppendChild(users.CreateCDataSection(s));
                                userNode.AppendChild(saltNode);

                                security.SaveUserConfiguration(Path, users.InnerXml);
                                
                                Login();

                                result = 1;
                            }
                            else
                            {
                                result = -7;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public List<User> GetAll()
        {
            var lstUser = new List<User>();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var users = security.ReadUserConfiguration(User.Current.Path);

                foreach (XmlNode node in users.SelectNodes("root/users/user"))
                {
                    var user = new User
                    {
                        UserName = node.SelectSingleNode("username").InnerText,
                        Password = node.SelectSingleNode("password").InnerText,
                        PublicKey = node.SelectSingleNode("publickey").InnerText,
                        PrivateKey = node.SelectSingleNode("privatekey").InnerText,
                        Email = node.SelectSingleNode("email").InnerText
                    };
                    lstUser.Add(user);
                }
            }

            return lstUser;
        }

        public string GetAllJSON()
        {
            string result = "";

            var lstUser = new List<User>();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var users = security.ReadUserConfiguration(User.Current.Path);

                foreach (XmlNode node in users.SelectNodes("root/users/user"))
                {
                    var user = new User
                    {
                        UserName = node.SelectSingleNode("username").InnerText,
                        Password = node.SelectSingleNode("password").InnerText,
                        PublicKey = node.SelectSingleNode("publickey").InnerText,
                        PrivateKey = node.SelectSingleNode("privatekey").InnerText,
                        Email = node.SelectSingleNode("email").InnerText
                    };
                    lstUser.Add(user);
                }

                result = new JavaScriptSerializer().Serialize(lstUser); 
            }

            return result;
        }

        public User GetBy(string name)
        {
            var user = new User();
            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var users = security.ReadUserConfiguration(User.Current.Path);

                var node = users.SelectSingleNode("/root/users/user[username='" + name + "']");
                if (node != null)
                {
                    user.UserName = node.SelectSingleNode("username").InnerText;
                    user.Password = node.SelectSingleNode("password").InnerText;
                    user.Email = node.SelectSingleNode("email").InnerText;
                    user.PublicKey = node.SelectSingleNode("publickey").InnerText;
                    user.PrivateKey = node.SelectSingleNode("privatekey").InnerText;
                }
            }
            else
            {
                user = null;
            }

            return user;
        }

        public string GetByJSON(string name)
        {
            string result = "";

            var user = new User();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var users = security.ReadUserConfiguration(User.Current.Path);

                var node = users.SelectSingleNode("/root/users/user[username='" + name + "']");
                if (node != null)
                {
                    user.UserName = node.SelectSingleNode("username").InnerText;
                    user.Password = node.SelectSingleNode("password").InnerText;
                    user.Email = node.SelectSingleNode("email").InnerText;
                    user.PublicKey = node.SelectSingleNode("publickey").InnerText;
                    user.PrivateKey = node.SelectSingleNode("privatekey").InnerText;
                }
                result = new JavaScriptSerializer().Serialize(user); 
            }

            return result;
        }

        public bool CheckUserAndEmailExists(string username, string email, XmlDocument doc)
        {
            bool result = false;

            foreach (XmlNode node in doc.SelectNodes("/root/users/user"))
            {
                if (node.SelectSingleNode("username") != null && node.SelectSingleNode("email") != null)
                {
                    string uname = node.SelectSingleNode("username").InnerText;
                    string em = node.SelectSingleNode("email").InnerText;

                    if (username == uname || em == email)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public bool Delete()
        {
            bool result = false;

            if (User.Current != null)
            {
                if (File.Exists(User.Current.Path))
                {
                    File.Delete(User.Current.Path);
                    result = true;
                }
            }

            return result;
        }

        public int Update()
        {
            int result = 0;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var users = security.ReadUserConfiguration(User.Current.Path);
                var node = users.SelectSingleNode("/root/users/user[username='" + UserName + "']");
                if (node != null)
                {
                    node.SelectSingleNode("username").InnerText = UserName;
                    node.SelectSingleNode("password").InnerText = Password;
                    node.SelectSingleNode("email").InnerText = Email;

                    security.SaveUserConfiguration(User.Current.Path, users.OuterXml);

                    result = 1;
                }
            }

            return result;
        }

        public List<User> JSONToList(string json) 
        {
            var result = new JavaScriptSerializer().Deserialize<List<User>>(json);
            return result;
        }

        public User JSONToObject(string json)
        {
            var result = new JavaScriptSerializer().Deserialize<User>(json);
            return result;
        }
    }
}
