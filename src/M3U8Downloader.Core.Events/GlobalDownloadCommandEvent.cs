using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    public class GlobalDownloadCommandEvent : PubSubEvent<GlobalDownloadCommandEventArgs>
    {
    }

    public class GlobalDownloadCommandEventArgs
    {
        public GlobalDownloadCommandEventArgs(Command conmmand)
        {
            DownloadCommand = conmmand;
        }
        public enum Command
        {
            START, STOP, REMOVE_FINISHED
        }

        public Command DownloadCommand
        {
            get; init;
        }
    }
}
