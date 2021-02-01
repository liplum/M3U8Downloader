using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    public class AllDownloadTasksCommandEvent : PubSubEvent<AllDownloadTasksCommandEventArgs>
    {
    }

    public class AllDownloadTasksCommandEventArgs
    {
        public AllDownloadTasksCommandEventArgs(Command conmmand)
        {
            TaskCommand = conmmand;
        }
        public enum Command
        {
            START, STOP, REMOVE_FINISHED
        }

        public Command TaskCommand
        {
            get; init;
        }
    }
}
