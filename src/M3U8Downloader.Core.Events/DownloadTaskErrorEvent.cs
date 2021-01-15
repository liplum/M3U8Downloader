using M3U8Downloader.Core.Models;
using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    public class DownloadTaskErrorEvent : PubSubEvent<DownloadTaskErrorEventArgs>
    {

    }

    public class DownloadTaskErrorEventArgs
    {
        public enum ErrorType
        {
            URI,
            TargetFolder,
        }

        public DownloadTaskErrorEventArgs(M3U8DownloadTask task, ErrorType errorType)
        {
            Task = task;
            Type = errorType;
        }
        public M3U8DownloadTask Task
        {
            get; init;
        }

        public ErrorType Type
        {
            get; init;
        }
    }
}
