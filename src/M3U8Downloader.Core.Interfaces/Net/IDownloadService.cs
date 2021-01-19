using M3U8Downloader.Core.Exceptions.DownloadExceptions;
using M3U8Downloader.Core.Models;
using System;
using System.Collections.Generic;

namespace M3U8Downloader.Core.Interfaces.Net
{
    public interface IDownloadService
    {
        public void DownloadDataFromTask(M3U8DownloadTask task, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null);

        public void DownloadDataFromSpans(IEnumerable<M3U8DownloadSpan> spans, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null);

        public void DownloadDataFromSpan(M3U8DownloadSpan span, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null);
    }
}
