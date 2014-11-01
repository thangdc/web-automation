using System;
using System.Net;

namespace WebAutomation.DownloadManager
{
    public class BaseProtocolProvider
    {
        static BaseProtocolProvider()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
        }

        protected WebRequest GetRequest(ResourceLocation location)
        {
            WebRequest request = WebRequest.Create(location.URL);
            request.Timeout = 30000;
            SetProxy(request);
            return request;
        }

        protected void SetProxy(WebRequest request)
        {
            if (Settings.Default.UseProxy)
            {
                WebProxy proxy = new WebProxy(Settings.Default.ProxyAddress, Settings.Default.ProxyPort);
                proxy.BypassProxyOnLocal = Settings.Default.ProxyByPassOnLocal;
                request.Proxy = proxy;

                if (!String.IsNullOrEmpty(Settings.Default.ProxyUserName))
                {
                    request.Proxy.Credentials = new NetworkCredential(
                        Settings.Default.ProxyUserName,
                        Settings.Default.ProxyPassword,
                        Settings.Default.ProxyDomain);
                }
            }
        }
    }
}
