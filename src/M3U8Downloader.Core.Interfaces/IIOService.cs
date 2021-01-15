using System.IO;

namespace M3U8Downloader.Core.Interfaces
{
    public interface IIOService
    {
        /// <summary>
        /// If the directory doesn't exist then create it.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns>
        /// Return true :When Existed or Create successfully.<br/>
        /// Return false:When Not Existed and create failed.
        /// </returns>
        public bool CheckExistedOrCreate(DirectoryInfo directory);
    }
}
