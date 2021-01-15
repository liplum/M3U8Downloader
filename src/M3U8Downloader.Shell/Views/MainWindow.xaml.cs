using M3U8Downloader.Core;
using MahApps.Metro.Controls;
using Prism.Regions;
using System.Windows;

namespace M3U8Downloader.Shell.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow(/*IRegionManager regionManager*/)
        {
            InitializeComponent();
/*            if (regionManager != null)
            {
                SetRegionManager(regionManager, null, RegionNames.Setting_Region);
            }*/
        }
        void SetRegionManager(IRegionManager regionManager, DependencyObject regionTarget, string regionName)
        {
            RegionManager.SetRegionName(regionTarget, regionName);
            RegionManager.SetRegionManager(regionTarget, regionManager);
        }
    }
}
