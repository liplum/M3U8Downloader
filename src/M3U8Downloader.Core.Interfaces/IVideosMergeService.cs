using System.Collections.Generic;

namespace M3U8Downloader.Core.Interfaces
{
    public interface IVideosMergeService
    {
        /// <summary>
        /// Merges a sequence of videos .
        /// </summary>
        /// <param name="paths">This sequence decides the order of the complete video merged.</param>
        /// <param name="outFileName">Complete path.</param>
        /// <returns>Whether the viode is merged successfully or not.</returns>
        public bool MergeVideo(List<string> paths, string outFileName);
    }
}
