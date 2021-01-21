using Prism.Commands;

namespace M3U8Downloader.Core.Interfaces.Global
{
    public interface IApplicationCommand
    {
        public CompositeCommand OpenSettingsCommand
        {
            get;
        }
    }
}
