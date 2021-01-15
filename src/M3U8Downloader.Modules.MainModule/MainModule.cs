using M3U8Downloader.Core;
using M3U8Downloader.Core.MVVM;
using M3U8Downloader.Modules.MainModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;

namespace M3U8Downloader.Modules.MainModule
{
    [Module(ModuleName = "MainModule")]
    [ModuleDependency("VideoMergeModule")]
    public class MainModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public MainModule(IRegionManager regionManager, IUnityContainer container) : base(container)
        {
            _regionManager = regionManager;
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
            _regionManager.RequestNavigate(RegionNames.DetailPage_Region, "DetailPage");
            _regionManager.RequestNavigate(RegionNames.Menu_Region, "Menu");
            _regionManager.RequestNavigate(RegionNames.TaskList_Region, "TaskList");
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.RegisterForNavigation<DetailPage>();
            containerRegistry.RegisterForNavigation<Menu>();
            containerRegistry.RegisterForNavigation<TaskList>();
        }
    }
}