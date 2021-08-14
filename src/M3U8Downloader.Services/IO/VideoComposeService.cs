using M3U8Downloader.Core.Exceptions.ComposeExceptions;
using M3U8Downloader.Core.Exceptions.DownloadExceptions;
using M3U8Downloader.Core.Interfaces.Cache;
using M3U8Downloader.Core.Interfaces.Global;
using M3U8Downloader.Core.Interfaces.IO;
using M3U8Downloader.Core.Interfaces.Tool;
using M3U8Downloader.Core.Models;
using System.Collections.Generic;
using System.IO;

namespace M3U8Downloader.Services.IO
{
    public class VideoComposeService : IVideoComposeService
    {
        private readonly ISpanDataCacheService _cacheService;
        private readonly IM3U8TaskCacheService _databaseService;
        private readonly IConfiguration _configuration;
        private readonly IDownloadTaskToolService _taskComparer;

        public VideoComposeService(ISpanDataCacheService cacheService, IM3U8TaskCacheService databaseService, IConfiguration configuration, IDownloadTaskToolService taskComparer)
        {
            _cacheService = cacheService;
            _databaseService = databaseService;
            _configuration = configuration;
            _taskComparer = taskComparer;
        }

        public List<string> ComposeVideo(M3U8DownloadTask task)
        {
            var targetFolder = new DirectoryInfo($@"{(_taskComparer.IsDefaultTargetFolder(task) ? _configuration.DefaultTargetDirectory : task.TargetFolder)}");
            //var cacheFolder = new DirectoryInfo($@"{_configuration.DefaultCacheSpansFilePath}\{task.FileName}");

            //Test
            var cacheFolder = new DirectoryInfo($@"{targetFolder}\Cache\{task.FileName}");
            //Test 
            try
            {
                if (!targetFolder.Exists)
                {
                    targetFolder.Create();
                }

                if (!cacheFolder.Exists)
                {
                    cacheFolder.Create();
                }
            }
            catch (IOException e)
            {
                throw new TargetFolderCreatException(task.FileName, e);
            }
            if (_databaseService.TryGetSpans(task, out var spans))
            {
                var filesList = new List<string>();
                foreach (var span in spans)
                {
                    var targetFile = @$"{cacheFolder.FullName}\{span.Number}.ts";
                    if (_cacheService.TryGetData(span, out var data))
                    {
                        using var fileStream = new FileStream(targetFile, FileMode.OpenOrCreate, FileAccess.Write);
                        fileStream.Write(data, 0, data.Length);
                        filesList.Add(targetFile);
                        _cacheService.TryRemoveData(span);
                    }
                }
                return filesList;
            }
            throw new VideoComposeException(task.FileName);
        }
    }
}
