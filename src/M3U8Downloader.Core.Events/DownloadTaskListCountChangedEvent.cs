using M3U8Downloader.Core.Models;
using Prism.Events;
using System.Collections.Generic;

namespace M3U8Downloader.Core.Events
{
    /// <summary>
    /// It'll raised when the list of Download Task changed.
    /// </summary>
    public class DownloadTaskListCountChangedEvent : PubSubEvent<DownloadTaskListCountChangedEventArgs>
    {

    }



    public class DownloadTaskListCountChangedEventArgs
    {
        public enum ChangedMode
        {
            REMOVED, ADDED,
            /// <summary>
            /// It's default.
            /// </summary>
            UNDEFINED,
        }

        public DownloadTaskListCountChangedEventArgs(int currentCount)
        {
            CurrentCount = currentCount;
        }

        public M3U8DownloadTask ChangedTask
        {
            get; init;
        }

        public IEnumerable<M3U8DownloadTask> ChangedTasks
        {
            get; init;
        }

        public bool IsChangedCountSingular
        {
            get; init;
        }

        public int CurrentCount
        {
            get; init;
        }

        public ChangedMode Mode
        {
            get; init;
        } = ChangedMode.UNDEFINED;
    }
}
