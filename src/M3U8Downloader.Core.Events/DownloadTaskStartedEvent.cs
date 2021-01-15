using M3U8Downloader.Core.Models;
using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    public class DownloadTaskStartedEvent : PubSubEvent<DownloadTaskStartedEventArgs>
    {

    }

    public class DownloadTaskStartedEventArgs
    {
        public DownloadTaskStartedEventArgs(M3U8DownloadTask task)
        {
            Task = task;
        }
        public M3U8DownloadTask Task
        {
            get; init;
        }
    }
}
