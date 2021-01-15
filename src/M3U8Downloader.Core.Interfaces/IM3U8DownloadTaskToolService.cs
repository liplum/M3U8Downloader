using M3U8Downloader.Core.Models;
using System.Collections.Generic;

namespace M3U8Downloader.Core.Interfaces
{
    public interface IM3U8DownloadTaskToolService
    {
        public bool IsEmpty(M3U8DownloadTask task);
        public bool Equal(M3U8DownloadTask a, M3U8DownloadTask b);
        public bool HasEmpty(IEnumerable<M3U8DownloadTask> tasks);
        public bool IsValid(M3U8DownloadTask task);
        public bool IsDefaultTargetFolder(M3U8DownloadTask task);
        public bool IsUriValid(M3U8DownloadTask task);
        public bool IsTargetFolderValid(M3U8DownloadTask task);
        public bool CanStart(M3U8DownloadTask task);
        public bool CanRetry(M3U8DownloadTask task);
        public bool CanEdit(M3U8DownloadTask task);
        public bool IsStartedButNotInDownload(M3U8DownloadTask task);
        public bool CanSave(M3U8DownloadTask task);
        public bool CanStop(M3U8DownloadTask task);
        public string GetOutputFileFullName(M3U8DownloadTask task);
    }
}
