using M3U8Downloader.Core.Interfaces.Global;
using Prism.Commands;

namespace M3U8Downloader.Service.Services.Global
{
    public class ApplicationCommand : IApplicationCommand
    {
        public CompositeCommand OpenSettingsCommand { get; } = new CompositeCommand();
    }
}
