using M3U8Downloader.Core.Models;
using System.Collections.Generic;

namespace M3U8Downloader.Core.Interfaces
{
    public interface IM3U8TaskDatabaseService
    {
        public bool TryGetSpans(M3U8DownloadTask task, out IEnumerable<M3U8DownloadSpan> spans);
        public bool TryAddSpans(M3U8DownloadTask task, IEnumerable<M3U8DownloadSpan> spans);

        public bool TryRemoveSpans(M3U8DownloadTask task);

        public void Clear();
    }
}
