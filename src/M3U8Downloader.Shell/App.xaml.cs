using M3U8Downloader.Core;
using M3U8Downloader.Core.infrastructures;
using M3U8Downloader.Core.Interfaces;
using M3U8Downloader.Service.Services;
using M3U8Downloader.Shell.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace M3U8Downloader.Shell
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
            regionManager.RegisterViewWithRegion(RegionNames.Settings_Region,typeof(Settings));
        }
        //2
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();

            containerRegistry.RegisterSingleton<IM3U8DownloadTaskToolService, M3U8DownloadTaskToolService>();

            containerRegistry.RegisterSingleton<IDownloadTaskManageService, DownloadTaskManageService>();

            containerRegistry.RegisterSingleton<IM3U8FileContentAnalyseService, M3U8FileContentAnalyseService>();

            containerRegistry.RegisterSingleton<IUriService, UriService>();

            containerRegistry.RegisterSingleton<ICacheService, CacheService>();

            containerRegistry.RegisterSingleton<IConfiguration, Configuration>();

            containerRegistry.RegisterSingleton<IM3U8TaskDatabaseService, M3U8TaskDatabaseService>();

            containerRegistry.RegisterSingleton<IDownloadService, HttpDownloadService>();

            containerRegistry.RegisterSingleton<IVideoComposeService, VideoComposeService>();

            containerRegistry.Register<IM3U8DownloadTaskComparer, M3U8DownloadTaskComparer>();

            containerRegistry.RegisterSingleton<ILocalizeHelperService, LocalizeHelperService>();

            containerRegistry.Register<IIOService, IOService>();

            containerRegistry.RegisterInstance(new HttpClient());


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
    }
}
