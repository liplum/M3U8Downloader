using M3U8Downloader.Core.Interfaces;
using M3U8Downloader.Core.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace M3U8Downloader.Service.Services
{
    public class CacheService : ICacheService
    {
        private readonly ConcurrentDictionary<M3U8DownloadSpan, byte[]> _dataBase = new ConcurrentDictionary<M3U8DownloadSpan, byte[]>();

        public CacheService()
        {

        }

        public bool AddData(M3U8DownloadSpan span, byte[] data)
        {
            return _dataBase.TryAdd(span, data);
        }

        public Task Prsistence()
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetData(M3U8DownloadSpan span, out byte[] data)
        {
            return _dataBase.TryGetValue(span, out data);
        }

        public void ClearCache()
        {
            _dataBase.Clear();
        }

        public bool TryRemoveData(M3U8DownloadSpan span)
        {
            return _dataBase.TryRemove(span, out _);
        }
    }
}
