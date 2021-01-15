using M3U8Downloader.Core.Models;
using Prism.Events;
using System.Collections.Generic;

namespace M3U8Downloader.Core.Events
{
    /// <summary>
    /// It'll be raised when you want to add any Download Task.
    /// </summary>
    public class DownloadTaskListNeedAddEvent : PubSubEvent<DownloadTaskListNeedAddEventArgs>
    {
    }

    public class DownloadTaskListNeedAddEventArgs
    {
        public DownloadTaskListNeedAddEventArgs()
        {

        }

        public IEnumerable<M3U8DownloadTask> NeedAdded
        {
            get; init;
        }

        /// <summary>
        /// It's default to add one task.
        /// </summary>
        public int Count
        {
            get; init;
        } = 1;
    }
}
