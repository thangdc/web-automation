using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml;
using ThangDC.Core.Securities;

namespace ThangDC.Core.Entities
{
    public class Account
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public List<Account> GetAll()
        {
            var lstAccount = new List<Account>();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var accounts = security.ReadAccountConfiguration(User.Current.Path);

                foreach (XmlNode node in accounts.SelectNodes("root/accounts/account"))
                {
                    Account account = new Account();
                    account.Name = node.SelectSingleNode("name").InnerText;
                    account.Username = node.SelectSingleNode("username").InnerText;
                    account.Password = node.SelectSingleNode("password").InnerText;
                    account.Description = node.SelectSingleNode("description").InnerText;

                    lstAccount.Add(account);
                }
            }

            return lstAccount;
        }

        public string GetAllJSON()
        {
            string result = "";

            var lstAccount = new List<Account>();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var accounts = security.ReadAccountConfiguration(User.Current.Path);

                foreach (XmlNode node in accounts.SelectNodes("root/accounts/account"))
                {
                    Account account = new Account();
                    account.Name = node.SelectSingleNode("name").InnerText;
                    account.Username = node.SelectSingleNode("username").InnerText;
                    account.Password = node.SelectSingleNode("password").InnerText;
                    account.Description = node.SelectSingleNode("description").InnerText;

                    lstAccount.Add(account);
                }

                result = new JavaScriptSerializer().Serialize(lstAccount); 
            }

            return result;
        }

        public Account GetBy(string name)
        {
            var account = new Account();
            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var accounts = security.ReadAccountConfiguration(User.Current.Path);

                var node = accounts.SelectSingleNode("/root/accounts/account[name='" + name + "']");
                if (node != null)
                {
                    account.Name = node.SelectSingleNode("name").InnerText;
                    account.Username = node.SelectSingleNode("username").InnerText;
                    account.Password = node.SelectSingleNode("password").InnerText;
                    account.Description = node.SelectSingleNode("description").InnerText;
                }
            }
            return account;
        }

        public string GetByJSON(string name)
        {
            string result = "";

            var account = new Account();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var accounts = security.ReadAccountConfiguration(User.Current.Path);

                var node = accounts.SelectSingleNode("/root/accounts/account[name='" + name + "']");
                if (node != null)
                {
                    account.Name = node.SelectSingleNode("name").InnerText;
                    account.Username = node.SelectSingleNode("username").InnerText;
                    account.Password = node.SelectSingleNode("password").InnerText;
                    account.Description = node.SelectSingleNode("description").InnerText;

                    result = new JavaScriptSerializer().Serialize(account); 
                }
            }

            return result;
        }

        public bool CheckExists(string name)
        {
            bool result = false;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var accounts = security.ReadAccountConfiguration(User.Current.Path);

                foreach (XmlNode node in accounts.SelectNodes("root/accounts/account"))
                {
                    string _name = node.SelectSingleNode("name").InnerText;

                    if (name == _name)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public int Add()
        {
            int result = 0;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);

                bool check = CheckExists(Name);

                if (check)
                {
                    result = -2;
                }
                else
                {

                    var accounts = security.ReadAccountConfiguration(User.Current.Path);

                    var node = accounts.SelectSingleNode("/root/accounts");
                    var accountNode = accounts.CreateElement("account");

                    node.AppendChild(accountNode);

                    var nameNode = accounts.CreateElement("name");
                    nameNode.AppendChild(accounts.CreateTextNode(Name));
                    accountNode.AppendChild(nameNode);

                    var userNode = accounts.CreateElement("username");
                    userNode.AppendChild(accounts.CreateTextNode(Username));
                    accountNode.AppendChild(userNode);

                    var passNode = accounts.CreateElement("password");
                    passNode.AppendChild(accounts.CreateTextNode(Password));
                    accountNode.AppendChild(passNode);

                    var desNode = accounts.CreateElement("description");
                    desNode.AppendChild(accounts.CreateTextNode(Description));
                    accountNode.AppendChild(desNode);

                    security.SaveAccountsConfiguration(User.Current.Path, accounts);

                    result = 1;
                }
            }
            else
            {
                result = -1;
            }

            return result;
        }

        public int Update()
        {
            int result = 0;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var accounts = security.ReadAccountConfiguration(User.Current.Path);

                var node = accounts.SelectSingleNode("/root/accounts/account[name='" + Name + "']");

                if (node != null)
                {
                    node.SelectSingleNode("username").InnerText = Username;
                    node.SelectSingleNode("password").InnerText = Password;
                    node.SelectSingleNode("description").InnerText = Description;
                    //node.SelectSingleNode("name").InnerText = Name;

                    security.SaveAccountsConfiguration(User.Current.Path, accounts);
                    result = 1;
                }
                else
                {
                    result = -2;
                }
            }
            else
            {
                result = -1;
            }

            return result;
        }

        public bool Delete()
        {
            bool result = false;

            if (User.Current != null)
            {

                var security = new Security(User.Current.Password);
                var accounts = security.ReadAccountConfiguration(User.Current.Path);

                var node = accounts.SelectSingleNode("/root/accounts/account[name='" + Name + "']");
                if (node != null)
                {
                    node.ParentNode.RemoveChild(node);
                    security.SaveAccountsConfiguration(User.Current.Path, accounts);
                    result = true;
                }
            }

            return result;
        }

        public List<Account> JSONToList(string json)
        {
            var result = new JavaScriptSerializer().Deserialize<List<Account>>(json);
            return result;
        }

        public Account JSONToObject(string json)
        {
            var result = new JavaScriptSerializer().Deserialize<Account>(json);
            return result;
        }
    }
}
