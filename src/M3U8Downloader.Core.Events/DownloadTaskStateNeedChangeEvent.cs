using M3U8Downloader.Core.Models;
using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    public class DownloadTaskStateNeedChangeEvent : PubSubEvent<DownloadTaskStateNeedChangeEventArgs>
    {

    }

    public class DownloadTaskStateNeedChangeEventArgs
    {
        public DownloadTaskStateNeedChangeEventArgs(M3U8DownloadTask downloadTask, NeedChangeMode needChangeMode)
        {
            Task = downloadTask;
            Mode = needChangeMode;
        }
        public enum NeedChangeMode
        {
            NEED_START, NEED_STOP,
        }

        public NeedChangeMode Mode
        {
            get; init;
        }

        public M3U8DownloadTask Task
        {
            get; init;
        }
    }
}
