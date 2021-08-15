using M3U8Downloader.Core.Models;

namespace M3U8Downloader.Core.infrastructures
{
    public class M3U8DownloadTaskComparer : IM3U8DownloadTaskComparer
    {
        public M3U8DownloadTaskComparer()
        {
        }
        public bool Equal(M3U8DownloadTask a, M3U8DownloadTask b)
        {
            return
                string.Compare(a.Uri, b.Uri) == 0 &&
                string.Compare(a.FileName, b.FileName) == 0 &&
                string.Compare(a.TargetFolder, b.TargetFolder) == 0
                ;
        }

        public bool IsEmpty(M3U8DownloadTask obj)
        {
            return
                string.IsNullOrEmpty(obj.Uri) &&
                string.IsNullOrEmpty(obj.FileName) &&
                string.IsNullOrEmpty(obj.TargetFolder) &&
                obj.State == TaskState.EDITING
                ;
        }
    }
}
