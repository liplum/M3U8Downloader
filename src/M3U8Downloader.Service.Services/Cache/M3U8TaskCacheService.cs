using M3U8Downloader.Core.Interfaces.Cache;
using M3U8Downloader.Core.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace M3U8Downloader.Service.Services.Cache
{
    public class M3U8TaskCacheService : IM3U8TaskCacheService
    {

        private readonly ConcurrentDictionary<M3U8DownloadTask, IEnumerable<M3U8DownloadSpan>> _database;

        public M3U8TaskCacheService()
        {
            _database = new ConcurrentDictionary<M3U8DownloadTask, IEnumerable<M3U8DownloadSpan>>();
        }

        public void Clear()
        {
            _database.Clear();
        }

        public bool TryAddSpans(M3U8DownloadTask task, IEnumerable<M3U8DownloadSpan> spans)
        {
            return _database.TryAdd(task, spans);
        }

        public bool TryGetSpans(M3U8DownloadTask task, out IEnumerable<M3U8DownloadSpan> spans)
        {
            return _database.TryGetValue(task, out spans);
        }

        public bool TryRemoveSpans(M3U8DownloadTask task)
        {
            return _database.TryRemove(task, out _);
        }
    }
}
