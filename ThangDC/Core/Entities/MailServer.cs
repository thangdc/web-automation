using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using ThangDC.Core.Securities;

namespace ThangDC.Core.Entities
{
    public class MailServer
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Server;
        public string Server
        {
            get { return _Server; }
            set { _Server = value; }
        }
        private int _Port;
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
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

        public List<MailServer> GetAll()
        {
            var listMailServer = new List<MailServer>();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var mails = security.ReadMailConfiguration(User.Current.Path);

                foreach (XmlNode node in mails.SelectNodes("root/mails/mail"))
                {
                    var mail = new MailServer
                    {
                        Name = node.SelectSingleNode("name").InnerText,
                        Server = node.SelectSingleNode("server").InnerText,
                        Port = int.Parse(node.SelectSingleNode("port").InnerText),
                        Username = node.SelectSingleNode("user").InnerText,
                        Password = node.SelectSingleNode("password").InnerText
                    };
                    listMailServer.Add(mail);
                }
            }

            return listMailServer;
        }

        public string GetAllJSON()
        {
            string result = "";

            var listMailServer = new List<MailServer>();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var mails = security.ReadMailConfiguration(User.Current.Path);

                foreach (XmlNode node in mails.SelectNodes("root/mails/mail"))
                {
                    var mail = new MailServer
                    {
                        Name = node.SelectSingleNode("name").InnerText,
                        Server = node.SelectSingleNode("server").InnerText,
                        Port = int.Parse(node.SelectSingleNode("port").InnerText),
                        Username = node.SelectSingleNode("user").InnerText,
                        Password = node.SelectSingleNode("password").InnerText
                    };
                    listMailServer.Add(mail);
                }

                result = new JavaScriptSerializer().Serialize(listMailServer); 
            }

            return result;
        }

        public MailServer GetBy(string name)
        {
            MailServer mailServer = null;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var mails = security.ReadMailConfiguration(User.Current.Path);

                XmlNode node = mails.SelectSingleNode("/root/mails/mail[name='" + name + "']");
                if (node != null)
                {
                    mailServer = new MailServer();
                    mailServer.Name = node.SelectSingleNode("name").InnerText;
                    mailServer.Server = node.SelectSingleNode("server").InnerText;
                    mailServer.Username = node.SelectSingleNode("user").InnerText;
                    mailServer.Password = node.SelectSingleNode("password").InnerText;

                    int.TryParse(node.SelectSingleNode("port").InnerText, out int port);

                    mailServer.Port = port;
                }
            }

            return mailServer;
        }

        public string GetByJSON(string name)
        {
            string result = "";

            MailServer mailServer = null;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                XmlDocument mails = security.ReadMailConfiguration(User.Current.Path);

                XmlNode node = mails.SelectSingleNode("/root/mails/mail[name='" + name + "']");
                if (node != null)
                {
                    mailServer = new MailServer
                    {
                        Name = node.SelectSingleNode("name").InnerText,
                        Server = node.SelectSingleNode("server").InnerText,
                        Username = node.SelectSingleNode("user").InnerText,
                        Password = node.SelectSingleNode("password").InnerText
                    };

                    int.TryParse(node.SelectSingleNode("port").InnerText, out int port);

                    mailServer.Port = port;

                    result = new JavaScriptSerializer().Serialize(mailServer); 
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
                var mails = security.ReadMailConfiguration(User.Current.Path);

                foreach (XmlNode node in mails.SelectNodes("root/mails/mail"))
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
                    bool isNum = int.TryParse(Port.ToString(), out int num);

                    if (isNum)
                    {
                        var mails = security.ReadMailConfiguration(User.Current.Path);

                        var node = mails.SelectSingleNode("/root/mails");
                        var mailNode = mails.CreateElement("mail");

                        node.AppendChild(mailNode);

                        var nameNode = mails.CreateElement("name");
                        nameNode.AppendChild(mails.CreateTextNode(Name));
                        mailNode.AppendChild(nameNode);

                        var serverNode = mails.CreateElement("server");
                        serverNode.AppendChild(mails.CreateTextNode(Server));
                        mailNode.AppendChild(serverNode);

                        var userNode = mails.CreateElement("user");
                        userNode.AppendChild(mails.CreateTextNode(Username));
                        mailNode.AppendChild(userNode);

                        var passNode = mails.CreateElement("password");
                        passNode.AppendChild(mails.CreateTextNode(Password));
                        mailNode.AppendChild(passNode);

                        var portNode = mails.CreateElement("port");
                        portNode.AppendChild(mails.CreateTextNode(Port.ToString()));
                        mailNode.AppendChild(portNode);

                        security.SaveMailConfiguration(User.Current.Path, mails.InnerXml);

                        result = 1;
                    }
                    else
                    {
                        result = -3;
                    }
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
                var mails = security.ReadMailConfiguration(User.Current.Path);

                var node = mails.SelectSingleNode("/root/mails/mail[name='" + Name + "']");
                if (node != null)
                {
                    node.SelectSingleNode("name").InnerText = Name;
                    node.SelectSingleNode("server").InnerText = Server;
                    node.SelectSingleNode("user").InnerText = Username;
                    node.SelectSingleNode("password").InnerText = Password;
                    node.SelectSingleNode("port").InnerText = Port.ToString();

                    security.SaveMailConfiguration(User.Current.Path, mails.InnerXml);

                    result = 1;
                }
            }
            else
            {
                result = -1;
            }

            return result;
        }

        public bool SendEmail(string email, string subject, string body)
        {
            bool result = false;

            try
            {
                var fromAddress = new MailAddress(Username);
                var toAddress = new MailAddress(email);

                var smtp = new SmtpClient
                {
                    Host = Server,
                    Port = Port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, Password),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    BodyEncoding = UTF8Encoding.UTF8
                })
                {
                    smtp.Send(message);
                    result = true;
                }
            }
            catch { result = false; }

            return result;
        }

        public bool Delete()
        {
            bool result = false;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var mails = security.ReadMailConfiguration(User.Current.Path);

                var node = mails.SelectSingleNode("/root/mails/mail[name='" + Name + "']");
                if (node != null)
                {
                    node.ParentNode.RemoveChild(node);
                    security.SaveMailConfiguration(User.Current.Path, mails.InnerXml);
                    result = true;
                }
            }

            return result;
        }

        public List<MailServer> JSONToList(string json)
        {
            var result = new JavaScriptSerializer().Deserialize<List<MailServer>>(json);
            return result;
        }

        public MailServer JSONToObject(string json)
        {
            var result = new JavaScriptSerializer().Deserialize<MailServer>(json);
            return result;
        }
    }
}
