using M3U8Downloader.Core.Interfaces.Global;
using static M3U8Downloader.Properties.Config;

namespace M3U8Downloader.Services.Global
{
    public class Configuration : IConfiguration
    {
        public string DefaultTargetDirectory
        {
            get => Default.DefaultTargetDirectory;
            set
            {
                if (Default.DefaultTargetDirectory.Equals(value)) return;
                Default.DefaultTargetDirectory = value;
                Save();
            }
        }
        public string DefaultVideoFormat
        {
            get => Default.DefaultVideoFormat;
            set
            {
                if (Default.DefaultVideoFormat.Equals(value)) return;
                Default.DefaultVideoFormat = value;
                Save();
            }
        }
        public string CacheSpanDirectory
        {
            get => Default.CacheSpanDirectory;
            set
            {
                if (Default.CacheSpanDirectory.Equals(value)) return;
                Default.CacheSpanDirectory = value;
                Save();
            }
        }

        public int MaxParallelCount
        {
            get => Default.MaxParallelCount;
            set
            {
                if (Default.MaxParallelCount.Equals(value)) return;
                Default.MaxParallelCount = value;
                Save();
            }
        }

        public void Save()
        {
            Default.Save();
        }
    }
}
