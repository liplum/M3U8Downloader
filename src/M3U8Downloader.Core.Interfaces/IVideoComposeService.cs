using M3U8Downloader.Core.Exceptions.ComposeExceptions;
using M3U8Downloader.Core.Exceptions.DownloadExceptions;
using M3U8Downloader.Core.Models;
using System.Collections.Generic;

namespace M3U8Downloader.Core.Interfaces
{
    public interface IVideoComposeService
    {
        /// <summary>
        /// Compose the video
        /// </summary>
        /// <param name="task">It is required to have already downloaded.</param>
        /// <returns></returns>
        /// <exception cref="TargetFolderCreatException"></exception>
        /// <exception cref="VideoComposeException"></exception>
        public List<string> ComposeVideo(M3U8DownloadTask task);
    }
}
