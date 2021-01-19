using M3U8Downloader.Core.Interfaces.IO;
using M3U8Downloader.Core.MVVM;
using M3U8Downloader.VideoMerge.Service;
using Prism.Ioc;
using Prism.Modularity;
using System.IO;
using System.Reflection;
using Unity;
using static M3U8Downloader.VideoMerge.Global;

namespace M3U8Downloader.VideoMerge
{
    [Module(ModuleName = "VideoMergeModule")]
    public class VideoMergeModule : ModuleBase
    {
        public const string FFMPEG_FILE_NAME = "ffmpeg.exe";
        public const string FFMPEG_FOLDER_NAME = "FFmpeg";

        private readonly FileInfo _ffmpegExe = null;
        private readonly FileInfo _tempFile = null;
        public VideoMergeModule(IUnityContainer container) : base(container)
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var exe = new FileInfo(@$"{path}\..\{FFMPEG_FOLDER_NAME}\{FFMPEG_FILE_NAME}");
            if (exe.Exists)
            {
                _ffmpegExe = exe;
            }
            _tempFile = new FileInfo(Path.GetTempFileName());
        }

        public override void OnInitialized(IContainerProvider containerProvider)
        {
            base.OnInitialized(containerProvider);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IVideosMergeService, VideosMergeService>();
            containerRegistry.RegisterInstance(_ffmpegExe, FFMPEG_EXE);
            containerRegistry.RegisterInstance(_tempFile, TEMPORARY_FILE);
        }

    }
}