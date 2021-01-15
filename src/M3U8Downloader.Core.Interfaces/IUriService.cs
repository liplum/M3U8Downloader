using M3U8Downloader.Core.Exceptions.UriExceptions;
using System;

namespace M3U8Downloader.Core.Interfaces
{
    public interface IUriService
    {
        /// <summary>
        /// Gets the content of m3u8 file via uri.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>The content split with '\n'</returns>
        /// <exception cref="CannotGetContentException"></exception>
        /// 
        public string GetM3U8Content(Uri uri);

        /// <summary>
        /// Gets the content of m3u8 file via uri.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>The content split with '\n'</returns>
        /// <exception cref="CannotGetContentException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// 
        public string GetM3U8Content(string uri);
    }
}
