using System.Windows.Forms;
using WebAutomation.DownloadManager;
namespace WebAutomation.UI
{
    public static class ClipboardHelper
    {
        public static string GetURLOnClipboard()
        {
            string url = string.Empty;

            if (Clipboard.ContainsText())
            {
                string tempUrl = Clipboard.GetText();

                if (ResourceLocation.IsURL(tempUrl))
                {
                    url = tempUrl;
                }
                else
                {
                    tempUrl = null;
                }
            }

            return url;
        }
    }
}
