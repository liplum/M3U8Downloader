using M3U8Downloader.Core.Exceptions.M3U8FileContentAnalysisExceptions;
using M3U8Downloader.Core.Models;
using System.Collections.Generic;

namespace M3U8Downloader.Core.Interfaces.Analysis
{
    public interface IM3U8FileContentAnalysisService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="m3u8FileContent"></param>
        /// <returns></returns>
        /// <exception cref="IsNotM3U8FileException"></exception>
        public IEnumerable<M3U8DownloadSpan> GetEveryDownloadSpan(M3U8DownloadTask task, string m3u8FileContent);
    }
}
