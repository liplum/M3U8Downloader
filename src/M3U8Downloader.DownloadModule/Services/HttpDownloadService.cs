using M3U8Downloader.Core.Exceptions.DownloadExceptions;
using M3U8Downloader.Core.Interfaces.Cache;
using M3U8Downloader.Core.Interfaces.Net;
using M3U8Downloader.Core.Models;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace M3U8Downloader.Download.Services
{
    public class HttpDownloadService : IDownloadService
    {
        private readonly IContainerProvider _provider;
        private readonly ISpanDataCacheService _cacheService;
        private readonly IM3U8TaskCacheService _taskDatabase;
        private readonly HttpClient _client;
        public HttpDownloadService(IContainerProvider containerProvider)
        {
            _provider = containerProvider;
            _cacheService = _provider.Resolve<ISpanDataCacheService>();
            _taskDatabase = _provider.Resolve<IM3U8TaskCacheService>();
            _client = _provider.Resolve<HttpClient>();
        }

        public void DownloadDataFromTask(M3U8DownloadTask task, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null)
        {
            _taskDatabase.TryGetSpans(task, out var spans);
            Parallel.ForEach(spans, (span, loopState) =>
            {
                if (span.State == SpanSate.FINISHED)
                {
                    loopState.Break();
                }
                try
                {
                    span.State = SpanSate.DOWNLOADING;
                    var result = _client.GetByteArrayAsync(span.Uri).Result;
                    span.State = SpanSate.FINISHED;
                    _cacheService.AddData(span, result);
                }
                catch (Exception e)
                {
                    span.State = SpanSate.ERROR;
                    exceptionHandler?.Invoke(span, new SpanDownloadException(span.Uri, e));
                    //coming soon
                    loopState.Break();
                }
            });
            return;
        }

        public void DownloadDataFromSpans(IEnumerable<M3U8DownloadSpan> spans, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null)
        {
            Parallel.ForEach(spans, (span, loopState) =>
            {
                if (span.State == SpanSate.FINISHED)
                {
                    loopState.Break();
                }
                try
                {
                    span.State = SpanSate.DOWNLOADING;
                    var result = _client.GetByteArrayAsync(span.Uri).Result;
                    span.State = SpanSate.FINISHED;
                    _cacheService.AddData(span, result);
                }
                catch (Exception e)
                {
                    span.State = SpanSate.ERROR;
                    exceptionHandler?.Invoke(span, new SpanDownloadException(span.Uri, e));
                    //coming soon
                    loopState.Break();
                }
            });
            return;
        }

        public void DownloadDataFromSpan(M3U8DownloadSpan span, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null)
        {
            if (span.State == SpanSate.FINISHED)
            {
                return;
            }
            try
            {
                var result = _client.GetByteArrayAsync(span.Uri).Result;
                span.State = SpanSate.FINISHED;
                _cacheService.AddData(span, result);
            }
            catch
            {
                span.State = SpanSate.ERROR;
                //coming soon
                return;
            }
        }
    }
}
