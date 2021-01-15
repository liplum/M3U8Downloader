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
        public DownloadTaskStateChangedEventArgs(ChangeMode changeMode)
        {
            Mode = changeMode;
        }
        public enum ChangeMode
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
        public ChangeMode Mode
        {
            get; init;
        }
    }
}
