using M3U8Downloader.Core.Models;
using System.Threading.Tasks;

namespace M3U8Downloader.Core.Interfaces
{
    public interface ICacheService
    {
        public bool TryGetData(M3U8DownloadSpan span, out byte[] data);
        public bool AddData(M3U8DownloadSpan span, byte[] data);

        public bool TryRemoveData(M3U8DownloadSpan span);

        public Task Prsistence();

        public void ClearCache();
    }
}
