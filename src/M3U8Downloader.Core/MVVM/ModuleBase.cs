using Prism.Ioc;
using Prism.Modularity;
using Unity;

namespace M3U8Downloader.Core.MVVM
{
    public abstract class ModuleBase : IModule
    {
        protected IUnityContainer Container
        {
            get;
        }

        protected ModuleBase(IUnityContainer container)
        {
            Container = container;
        }

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}
