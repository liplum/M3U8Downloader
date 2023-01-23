using M3U8Downloader.Core;
using M3U8Downloader.Core.Adapter;
using M3U8Downloader.Core.infrastructures;
using M3U8Downloader.Core.Interfaces.Cache;
using M3U8Downloader.Core.Interfaces.Global;
using M3U8Downloader.Core.Interfaces.IO;
using M3U8Downloader.Core.Interfaces.Manager;
using M3U8Downloader.Core.Interfaces.Net;
using M3U8Downloader.Core.Interfaces.Tool;
using M3U8Downloader.Services.Cache;
using M3U8Downloader.Services.Global;
using M3U8Downloader.Services.IO;
using M3U8Downloader.Services.Manager;
using M3U8Downloader.Services.Net;
using M3U8Downloader.Services.Tool;
using M3U8Downloader.Views;
using MahApps.Metro.Controls;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace M3U8Downloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        //3
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        //4
        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.Settings_Region, "Settings");
            //regionManager.RegisterViewWithRegion(RegionNames.Settings_Region, typeof(Settings));
        }
        //2
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();

            containerRegistry.RegisterSingleton<IDownloadTaskToolService, DownloadTaskToolService>();

            containerRegistry.RegisterSingleton<IDownloadTaskManageService, DownloadTaskManageService>();

            containerRegistry.RegisterSingleton<IUriService, UriService>();

            containerRegistry.RegisterSingleton<ISpanDataCacheService, SpanDataCacheService>();

            containerRegistry.RegisterSingleton<IConfiguration, Configuration>();

            containerRegistry.RegisterSingleton<IM3U8TaskCacheService, M3U8TaskCacheService>();

            containerRegistry.RegisterSingleton<IVideoComposeService, VideoComposeService>();

            containerRegistry.RegisterSingleton<IM3U8DownloadTaskComparer, M3U8DownloadTaskComparer>();

            containerRegistry.RegisterSingleton<IDownloadTaskStateManageService, DownloadTaskStateMachineService>();

            containerRegistry.RegisterSingleton<ILocalizeHelperService, LocalizeHelperService>();

            containerRegistry.RegisterSingleton<IApplicationCommand, ApplicationCommand>();

            containerRegistry.RegisterSingleton<IIOService, IOService>();

            containerRegistry.RegisterInstance(new HttpClient());

            containerRegistry.RegisterForNavigation<Settings>();

        }

        //1
        protected override IModuleCatalog CreateModuleCatalog()
        {
            var modulePath = @".\Modules";
            if (!Directory.Exists(modulePath))
            {
                Directory.CreateDirectory(modulePath);
            }
            return new DirectoryModuleCatalog() { ModulePath = modulePath };
        }
        //5
        protected override void OnInitialized()
        {
            base.OnInitialized();
            //TODO:Read history to IDownloadTaskManageService from the file.
            LocalizeDictionary.Instance.Culture = Thread.CurrentThread.CurrentCulture;
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping<FlyoutsControl, FlyoutsControlAdapter>();
        }
    }
}
