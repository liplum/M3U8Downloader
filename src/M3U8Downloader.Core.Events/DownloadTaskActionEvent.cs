using M3U8Downloader.Core.Models;
using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    public class DownloadTaskActionEvent : PubSubEvent<DownloadTaskActionEventArgs>
    {

    }

    public class DownloadTaskActionEventArgs
    {
        public DownloadTaskActionEventArgs(M3U8DownloadTask downloadTask, Action action)
        {
            Task = downloadTask;
            ActionType = action;
        }
        public enum Action
        {
            NEED_START
        }

        public Action ActionType
        {
            get; init;
        }

        public M3U8DownloadTask Task
        {
            get; init;
        }
    }
}
