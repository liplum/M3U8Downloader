using M3U8Downloader.Core.Interfaces.Net;
using M3U8Downloader.Core.MVVM;
using M3U8Downloader.Download.Services;
using Prism.Ioc;
using Prism.Modularity;
using Unity;

namespace M3U8Downloader.Download
{
    [Module(ModuleName = "DownloadModule")]
    public class DownloadModule : ModuleBase
    {
        public DownloadModule(IUnityContainer container) : base(container)
        {
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IDownloadService, HttpDownloadService>();
        }
    }
}
