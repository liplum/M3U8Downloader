using Prism.Mvvm;
using Prism.Navigation;

namespace M3U8Downloader.Core.MVVM
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
