using Prism.Events;

namespace M3U8Downloader.Core.Events
{
    public class WindowSizeChangedEvent : PubSubEvent<WindowSizeChangedEventArgs>
    {

    }

    public class WindowSizeChangedEventArgs
    {
        public double ActualWidth
        {
            get; init;
        }
        public double ActualHeight
        {
            get; init;

        }
    }
}
