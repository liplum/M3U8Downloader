namespace M3U8Downloader.Core.Interfaces.Global
{
    public interface IConfiguration
    {
        public void Save();

        public string DefaultTargetDirectory
        {
            get; set;
        }

        public string DefaultVideoFormat
        {
            get; set;
        }

        public string CacheSpanDirectory
        {
            get; set;
        }

        public int MaxParallelCount
        {
            get; set;
        }
    }
}
