using M3U8Downloader.Core.Interfaces;
using System.IO;

namespace M3U8Downloader.Service.Services
{
    public class IOService : IIOService
    {
        public bool CheckExistedOrCreate(DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                return true;
            }
            else
            {
                try
                {
                    directory.Create();
                    return true;
                }
                catch (IOException)
                {
                    return false;
                }
            }
        }
    }
}
