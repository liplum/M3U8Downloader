namespace M3U8Downloader.Core.Interfaces
{
    public interface ILocalizeHelperService
    {
        public string GetLocalizedString(string assemblyName, string key, string resourceFileName = "Resources");
    }
}
