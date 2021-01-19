using M3U8Downloader.Core.Models;
using Prism.Events;
using System.Collections.Generic;

namespace M3U8Downloader.Core.Events
{
    public class DownloadTaskStateChangedEvent : PubSubEvent<DownloadTaskStateChangedEventArgs>
    {
    }

    public class DownloadTaskStateChangedEventArgs
    {
        public DownloadTaskStateChangedEventArgs(ChangeType changeType)
        {
            Type = changeType;
        }
        public enum ChangeType
        {
            STARTED, STOPPED, FINISHED
        }

        public M3U8DownloadTask Task
        {
            get; init;
        }
        public IEnumerable<M3U8DownloadTask> Tasks
        {
            get; init;
        }
        public ChangeType Type
        {
            get; init;
        }
    }
}
