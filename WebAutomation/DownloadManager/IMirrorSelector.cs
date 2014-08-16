using System.Reflection;
namespace WebAutomation.DownloadManager
{
    public interface IMirrorSelector
    {
        void Init(Downloader downloader);

        ResourceLocation GetNextResourceLocation();
    }
}
