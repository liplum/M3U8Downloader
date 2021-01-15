using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    public class AllDownloadTasksStateNeedChangeEvent : PubSubEvent<AllDownloadTasksStateNeedChangeEventArgs>
    {
    }

    public class AllDownloadTasksStateNeedChangeEventArgs
    {
        public AllDownloadTasksStateNeedChangeEventArgs(NeedChangeMode needChangeMode)
        {
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
    }
}
