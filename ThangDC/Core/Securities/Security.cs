using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using ThangDC.Core.Entities;
using ThangDC.Core.WebAutomation;
namespace ThangDC.Core.Securities
{
    public class Security
    {
        private string _Password = "$1!3#554$7^5%8&(8)*&iajbjasf3jhsd8*3k0JDAD123!";
        private string connectioncfg = "connection.cfg";
        private string usercfg = "user.cfg";
        private string rolecfg = "role.cfg";
        private string mailcfg = "mail.cfg";
        private string accountcfg = "account.cfg";
        
        public Security(string password)
        {
            _Password = password;
        }

        public void SaveConnectionConfiguration(string path, string connectionName, string server, string dbname, string dbuser, string dbpassword, string provider)
        {
            var zip = new Zip();
            string content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><connections><connection><name>" + connectionName + "</name><server>" + server + "</server><dbname>" + dbname + "</dbname><dbuser>" + dbuser + "</dbuser><dbpass>" + dbpassword + "</dbpass><provider>" + provider + "</provider></connection></connections></root>";
            zip.Save(path, connectioncfg, content, _Password);
        }

        public void SaveConnectionConfiguration(string path, string content)
        {
            var zip = new Zip();
            zip.Save(path, connectioncfg, content, _Password);
        }

        public void SaveUserConfiguration(string path, string username, string email, string password, string privatekey, string publickey, string salt)
        {
            var zip = new Zip();
            string content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><users><user><username>" + username + "</username><email>" + email + "</email><password><![CDATA[" + password + "]]></password><privatekey><![CDATA[" + privatekey + "]]></privatekey><publickey><![CDATA[" + publickey + "]]></publickey><salt><![CDATA[" + salt + "]]></salt></user></users></root>";
            zip.Save(path, usercfg, content, _Password);
        }

        public void SaveUserConfiguration(string path, string content)
        {
            var zip = new Zip();
            zip.Save(path, usercfg, content, _Password);
        }

        public void SaveRoleConfiguration(string path, string role, string description)
        {
            var zip = new Zip();
            var content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><roles><role><name>" + role + "</name><description>" + description + "</description></role></roles></root>";
            zip.Save(path, rolecfg, content, _Password);
        }

        public void SaveMailConfiguration(string path, string mailname, string server, int port, string user, string password)
        {
            var zip = new Zip();
            var content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><mails><mail><name>" + mailname + "</name><server>" + server + "</server><port>" + port + "</port><user>" + user + "</user><password>" + password + "</password></mail></mails></root>";
            zip.Save(path, mailcfg, content, _Password);
        }

        public void SaveMailConfiguration(string path, string content)
        {
            var zip = new Zip();
            zip.Save(path, mailcfg, content, _Password);
        }

        public void SaveAccountsConfiguration(string path, string name, string username, string password, string description)
        {
            var zip = new Zip();
            string content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><accounts><account><name>" + name + "</name><username>" + username + "</username><password>" + password + "</password><description>" + description + "</description></account></accounts></root>";
            zip.Save(path, accountcfg, content, _Password);
        }

        public XmlDocument ReadConnectionConfiguration(string path)
        {
            var zip = new Zip();
            var result = zip.ReadXML(path, connectioncfg, _Password);

            return result;
        }

        public XmlDocument ReadUserConfiguration(string path)
        {
            var zip = new Zip();
            var result = zip.ReadXML(path, usercfg, _Password);

            return result;
        }

        public XmlDocument ReadRoleConfiguration(string path)
        {
            var zip = new Zip();
            var result = zip.ReadXML(path, rolecfg, _Password);

            return result;
        }

        public XmlDocument ReadMailConfiguration(string path)
        {
            var zip = new Zip();
            var result = zip.ReadXML(path, mailcfg, _Password);

            return result;
        }

        public XmlDocument ReadAccountConfiguration(string path)
        {
            var zip = new Zip();
            var result = zip.ReadXML(path, accountcfg, _Password);

            return result;
        }

        public void SaveAccountsConfiguration(string path, XmlNode document)
        {
            var zip = new Zip();
            var content = document.InnerXml;
            zip.Save(path, accountcfg, content, _Password);
        }

        public void SaveConnection(string path, XmlNode document)
        {
            var zip = new Zip();
            string content = document.InnerXml;
            zip.Save(path, connectioncfg, content, _Password);
        }

        public void SaveUsersConfiguration(string path, XmlNode document)
        {
            var zip = new Zip();
            string content = document.InnerXml;
            zip.Save(path, usercfg, content, _Password);
        }

        public string encrypt(string publicKey, string plainText)
        {
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            byte[] plainBytes = null;
            byte[] encryptedBytes = null;

            string result = "";
            try
            {
                cspParams = new CspParameters
                {
                    ProviderType = 1
                };
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(publicKey);

                plainBytes = Encoding.UTF8.GetBytes(plainText);
                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);
                result = Convert.ToBase64String(encryptedBytes);
            }
            catch (System.Exception ex) { }
            return result;
        }

        public string decrypt(string privateKey, string encrypted)
        {
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            byte[] encryptedBytes = null;
            byte[] plainBytes = null;

            string result = "";
            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(privateKey);

                encryptedBytes = Convert.FromBase64String(encrypted);
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

                result = Encoding.UTF8.GetString(plainBytes);
            }
            catch (System.Exception ex) { }
            return result;
        }

        public void HashPassword(string password, out byte[] salt, out byte[] hashPassword)
        {
            salt = new byte[16];
            hashPassword = new byte[16];

            HashAlgorithm hashAlgorithm = SHA512.Create();
            byte[] tmp = new byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(tmp);
            salt = tmp;
            List<byte> pass = new List<byte>(Encoding.Unicode.GetBytes(password));
            pass.AddRange(tmp);
            byte[] result = hashAlgorithm.ComputeHash(pass.ToArray());
            hashPassword = result;
        }

        public bool CheckPassword(string hashPassword, byte[] salt, byte[] password)
        {
            bool result = false;

            HashAlgorithm hashAlgorithm = SHA512.Create();
            List<byte> buffer = new List<byte>(Encoding.Unicode.GetBytes(hashPassword));
            buffer.AddRange(salt);
            byte[] computedHash = hashAlgorithm.ComputeHash(buffer.ToArray());

            result = (computedHash.Length == password.Length);
            if (result)
            {
                for (int i = 0; i < computedHash.Length; i++)
                {
                    result &= computedHash[i] == password[i];
                    if (!result) break;
                }
            }

            return result;
        }

    }
}
