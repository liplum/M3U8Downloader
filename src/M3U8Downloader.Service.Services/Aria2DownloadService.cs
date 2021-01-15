using M3U8Downloader.Core.Exceptions.DownloadExceptions;
using M3U8Downloader.Core.Interfaces;
using M3U8Downloader.Core.Models;
using System;
using System.Collections.Generic;

namespace M3U8Downloader.Service.Services
{
    public class Aria2DownloadService : IDownloadService
    {
        public void DownloadDataFrom(IEnumerable<M3U8DownloadSpan> spans, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null)
        {
            throw new NotImplementedException();
        }

        public void DownloadDataFrom(M3U8DownloadSpan span, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null)
        {
            throw new NotImplementedException();
        }

    }
}
