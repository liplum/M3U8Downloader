using M3U8Downloader.Core.Interfaces.Analysis;
using M3U8Downloader.Core.MVVM;
using M3U8Downloader.FileContentAnalysis.Service;
using Prism.Ioc;
using Prism.Modularity;
using Unity;

namespace M3U8Downloader.FileContentAnalysis
{
    [Module(ModuleName = "FileContentAnalysisModule")]
    public class FileContentAnalysisModule : ModuleBase
    {
        public FileContentAnalysisModule(IUnityContainer container) : base(container)
        {
        }
        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IM3U8FileContentAnalysisService, M3U8AnalysisService>();
        }
    }
}
