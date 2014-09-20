
namespace ThangDC.Core.WebAutomation
{
    using System.IO;
    using System.Xml;

    public class Zip
    {
        public void Save(string path, string fileName, string content, string password)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(path))
            {
                zip.Password = password;
                zip.Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes256;

                if (zip["" + fileName + ""] != null)
                {
                    zip.RemoveEntry(fileName);
                }
                
                zip.AddEntry(fileName, content, System.Text.Encoding.UTF8);
                zip.Save(path);
            }
        }

        public string Read(string path, string fileName, string password)
        {
            string result = "";
            try
            {
                MemoryStream ms = new MemoryStream();
                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(path))
                {
                    Ionic.Zip.ZipEntry e = zip["" + fileName + ""];
                    e.ExtractWithPassword(ms, password);
                    StreamReader sr = new StreamReader(ms);
                    ms.Position = 0;
                    result = sr.ReadToEnd();
                }
            }
            catch { }
            return result;
        }

        public XmlDocument ReadXML(string path, string fileName, string password)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                string xml = Read(path, fileName, password);
                doc.LoadXml(xml);
            }
            catch { }

            return doc;
        }
    }
}
