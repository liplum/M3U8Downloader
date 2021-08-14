using M3U8Downloader.Core.Interfaces.IO;
using Prism.Ioc;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static M3U8Downloader.VideoMerge.Global;


namespace M3U8Downloader.VideoMerge.Service
{
    public class VideosMergeService : IVideosMergeService
    {
        private readonly IContainerProvider _provider;
        private readonly FileInfo FFmpegExe;
        private readonly FileInfo TemporaryFile;
        private readonly object _lock = new object();
        public VideosMergeService(IContainerProvider provider)
        {
            _provider = provider;
            FFmpegExe = _provider.Resolve<FileInfo>(FFMPEG_EXE);
            TemporaryFile = _provider.Resolve<FileInfo>(TEMPORARY_FILE);
        }

        public bool MergeVideo(List<string> paths, string outFileName)
        {
            lock (_lock)
            {
                using (var filesList = new FileStream(TemporaryFile.FullName, FileMode.Truncate, FileAccess.Write))
                {
                    using var writer = new StreamWriter(filesList);
                    foreach (var path in paths)
                    {
                        writer.WriteLine($"file '{path}'");
                    }
                }
                var exe = new Process();
                exe.StartInfo.FileName = FFmpegExe.FullName;
                exe.StartInfo.CreateNoWindow = true;
                exe.StartInfo.Arguments =
                    $"-threads 0 -f concat -safe 0 -i \"{TemporaryFile.FullName}\" -c copy \"{outFileName}\"";
                exe.Start();
                exe.WaitForExit();
                return exe.ExitCode == 0;
            }
        }
    }
}
