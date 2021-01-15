﻿using M3U8Downloader.Core.Interfaces;
using WPFLocalizeExtension.Extensions;

namespace M3U8Downloader.Service.Services
{
    public class LocalizeHelperService : ILocalizeHelperService
    {
        public string GetLocalizedString(string assemblyName, string key, string resourceFileName = "Resources")
        {
            var fullKey = $"{assemblyName}:{resourceFileName}:{key}";
            var locExtension = new LocExtension(fullKey);
            locExtension.ResolveLocalizedValue(out string localizedString);
            return localizedString;
        }
    }
}
