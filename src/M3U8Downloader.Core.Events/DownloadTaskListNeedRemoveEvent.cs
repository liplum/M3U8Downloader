using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    /// <summary>
    /// It'll be raised when you want to remove any Download Task.
    /// </summary>
    public class DownloadTaskListNeedRemoveEvent : PubSubEvent<DownloadTaskListNeedRemoveEventArgs>
    {
    }

    public class DownloadTaskListNeedRemoveEventArgs
    {

        public DownloadTaskListNeedRemoveEventArgs()
        {
        }
        /// <summary>
        /// It's default to remove the task selected currently when the index is -1 .
        /// </summary>
        public int Index
        {
            get; init;
        } = -1;
    }
}
