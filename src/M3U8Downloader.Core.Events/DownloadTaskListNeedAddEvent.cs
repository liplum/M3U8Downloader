using Prism.Events;

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

        /// <summary>
        /// It's default to add one task.
        /// </summary>
        public int Count
        {
            get; init;
        } = 1;
    }
}
