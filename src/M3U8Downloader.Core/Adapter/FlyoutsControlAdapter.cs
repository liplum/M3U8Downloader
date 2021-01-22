using MahApps.Metro.Controls;
using Prism.Regions;
using System.Collections.Specialized;
using System.Windows;

namespace M3U8Downloader.Core.Adapter
{
    public class FlyoutsControlAdapter : RegionAdapterBase<FlyoutsControl>
    {
        public FlyoutsControlAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {

        }

        protected override void Adapt(IRegion region, FlyoutsControl regionTarget)
        {
            region.ActiveViews.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement elemt in e.NewItems)
                    {
                        regionTarget.Items.Add(new Flyout
                        {
                            Content = elemt,
                            DataContext = elemt.DataContext
                        });
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
