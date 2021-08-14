using M3U8Downloader.Core.infrastructures;
using M3U8Downloader.Core.Interfaces.IO;
using M3U8Downloader.Core.Interfaces.Tool;
using M3U8Downloader.Core.Models;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.IO;

namespace M3U8Downloader.Services.Tool
{
    public class DownloadTaskToolService : IDownloadTaskToolService
    {
        public DownloadTaskToolService(IContainerProvider containerProvider)
        {
            _provider = containerProvider;
            _comparer = _provider.Resolve<IM3U8DownloadTaskComparer>();
            _IO = _provider.Resolve<IIOService>();
        }
        private readonly IM3U8DownloadTaskComparer _comparer;
        private readonly IIOService _IO;
        private readonly IContainerProvider _provider;

        public bool Equal(M3U8DownloadTask a, M3U8DownloadTask b)
        {
            return _comparer.Equal(a, b);
        }

        public bool HasEmpty(IEnumerable<M3U8DownloadTask> tasks)
        {
            foreach (var task in tasks)
            {
                if (IsEmpty(task))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsDefaultTargetFolder(M3U8DownloadTask task)
        {
            //It should compare with i18n string .
            return string.Equals(task.TargetFolder, "Default");
        }

        public bool IsEmpty(M3U8DownloadTask task)
        {
            return _comparer.IsEmpty(task);
        }

        public bool IsValid(M3U8DownloadTask task)
        {
            return IsUriValid(task) && IsTargetFolderValid(task);
        }

        public bool IsUriValid(M3U8DownloadTask task)
        {
            try
            {
                var uri = new Uri(task.Uri);
                return true;
            }
            catch (UriFormatException)
            {
                return false;
            }
        }

        public bool CanStart(M3U8DownloadTask task)
        {
            var state = task.State;
            return state == TaskState.NOT_STARTED || state == TaskState.EDITING;
        }

        public bool IsStartedButNotInDownload(M3U8DownloadTask task)
        {
            return task.State == TaskState.STARTED;
        }

        public bool IsTargetFolderValid(M3U8DownloadTask task)
        {
            return _IO.CheckExistedOrCreate(new DirectoryInfo(task.TargetFolder));
        }

        public string GetOutputFileFullName(M3U8DownloadTask task)
        {
            return $@"{task.TargetFolder}\{task.FileName}";
        }

        public bool CanRetry(M3U8DownloadTask task)
        {
            return task.State == TaskState.ERROR;
        }
    }
}