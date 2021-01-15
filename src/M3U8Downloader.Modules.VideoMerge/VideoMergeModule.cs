using M3U8Downloader.Core.Interfaces;
using M3U8Downloader.Modules.VideoMerge.Service;
using Prism.Ioc;
using Prism.Modularity;
using System.IO;
using System.Reflection;
using static M3U8Downloader.Modules.VideoMerge.Global;

namespace M3U8Downloader.Modules.VideoMerge
{
    [Module(ModuleName = "VideoMergeModule")]
    public class VideoMergeModule : IModule
    {
        public const string FFMPEG_FILE_NAME = "ffmpeg.exe";
        public const string FFMPEG_FOLDER_NAME = "FFmpeg";

        private readonly FileInfo _ffmpegExe = null;
        private readonly FileInfo _tempFile = null;
        public VideoMergeModule()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var exe = new FileInfo(@$"{path}\..\{FFMPEG_FOLDER_NAME}\{FFMPEG_FILE_NAME}");
            if (exe.Exists)
            {
                _ffmpegExe = exe;
            }
            _tempFile = new FileInfo(Path.GetTempFileName());
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IVideosMergeService, VideosMergeService>();
            containerRegistry.RegisterInstance(_ffmpegExe, FFMPEG_EXE);
            containerRegistry.RegisterInstance(_tempFile, TEMPORARY_FILE);
        }

    }
}