using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebAutomation.DownloadManager
{
    public class HttpProtocolProvider : BaseProtocolProvider, IProtocolProvider
    {
        static HttpProtocolProvider()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(certificateCallBack);
        }

        static bool certificateCallBack(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void FillCredentials(HttpWebRequest request, ResourceLocation rl)
        {
            if (rl.Authenticate)
            {
                string login = rl.Login;
                string domain = string.Empty;

                int slashIndex = login.IndexOf('\\');

                if (slashIndex >= 0)
                {
                    domain = login.Substring(0, slashIndex);
                    login = login.Substring(slashIndex + 1);
                }

                NetworkCredential myCred = new NetworkCredential(login, rl.Password);
                myCred.Domain = domain;

                request.Credentials = myCred;
            }
        }

        #region IProtocolProvider Members

        public virtual void Initialize(Downloader downloader)
        {
        }

        public virtual Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition)
        {
            HttpWebRequest request = (HttpWebRequest)GetRequest(rl);

            FillCredentials(request, rl);

            if (initialPosition != 0)
            {
                if (endPosition == 0)
                {
                    request.AddRange((int)initialPosition);
                }
                else
                {
                    request.AddRange((int)initialPosition, (int)endPosition);
                }
            }

            WebResponse response = request.GetResponse();

            return response.GetResponseStream();
        }

        public virtual RemoteFileInfo GetFileInfo(ResourceLocation rl, out Stream stream)
        {
            HttpWebRequest request = (HttpWebRequest)GetRequest(rl);

            FillCredentials(request, rl);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            RemoteFileInfo result = new RemoteFileInfo();
            result.MimeType = response.ContentType;
            result.LastModified = response.LastModified;
            result.FileSize = response.ContentLength;
            result.AcceptRanges = String.Compare(response.Headers["Accept-Ranges"], "bytes", true) == 0;

            stream = response.GetResponseStream();

            return result;
        }

        #endregion
    }
}
