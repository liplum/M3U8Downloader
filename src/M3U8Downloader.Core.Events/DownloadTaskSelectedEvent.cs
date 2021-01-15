using M3U8Downloader.Core.Models;
using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    /// <summary>
    /// It'll raised when you selected a new Download Task.
    /// </summary>
    public class DownloadTaskSelectedEvent : PubSubEvent<DownloadTaskSelectedEventArgs>
    {

    }

    public class DownloadTaskSelectedEventArgs
    {
        public DownloadTaskSelectedEventArgs(M3U8DownloadTask task)
        {
            SelectedDownloadTask = task;
        }
        public M3U8DownloadTask SelectedDownloadTask
        {
            get; init;
        }
    }
}
