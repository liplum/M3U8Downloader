using M3U8Downloader.Core.Exceptions.DownloadExceptions;
using M3U8Downloader.Core.Interfaces;
using M3U8Downloader.Core.Models;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace M3U8Downloader.Service.Services
{
    public class HttpDownloadService : IDownloadService
    {
        private readonly IContainerProvider _containerProvider;
        private readonly ICacheService _cacheService;
        private readonly HttpClient _client;
        public HttpDownloadService(IContainerProvider containerProvider, ICacheService dataBaseService)
        {
            _containerProvider = containerProvider;
            _cacheService = dataBaseService;
            _client = _containerProvider.Resolve<HttpClient>();
        }

        public void DownloadDataFrom(IEnumerable<M3U8DownloadSpan> spans, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null)
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
        public void DownloadDataFrom(M3U8DownloadSpan span, Action<M3U8DownloadSpan, SpanDownloadException> exceptionHandler = null)
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
