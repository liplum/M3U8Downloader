﻿namespace M3U8Downloader.Core.Interfaces
{
    public interface IConfiguration
    {
        public string DefaultTargetFolderPath
        {
            get; set;
        }

        public string DefaultVideoFormat
        {
            get; set;
        }

        public string DefaultCacheSpansFilePath
        {
            get; set;
        }
    }
}