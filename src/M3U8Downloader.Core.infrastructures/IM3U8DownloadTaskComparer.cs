using M3U8Downloader.Core.Models;

namespace M3U8Downloader.Core.infrastructures
{
    public interface IM3U8DownloadTaskComparer
    {
        public bool IsEmpty(M3U8DownloadTask obj);

        public bool Equal(M3U8DownloadTask a, M3U8DownloadTask b);
    }
}
