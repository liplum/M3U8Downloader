using M3U8Downloader.Core.Exceptions.UriExceptions;
using M3U8Downloader.Core.Interfaces.Net;
using System;
using System.IO;
using System.Net.Http;

namespace M3U8Downloader.Services.Net
{
    public class UriService : IUriService
    {
        private const string HTTP = "http";
        private const string HTTPS = "https";
        private const string FILE = "file";
        private const string FTP = "ftp";
        public string GetM3U8Content(Uri uri)
        {
            Uri _uri = null;
            if (!uri.IsAbsoluteUri)
            {
                _uri = new Uri(uri.AbsoluteUri);
            }
            else
            {
                _uri = uri;
            }
            switch (_uri.Scheme)
            {
                case HTTP:
                case HTTPS:
                    {
                        try
                        {
                            using var httpClient = new HttpClient();
                            var httpContent = httpClient.GetAsync(uri).Result.Content;
                            return httpContent.ReadAsStringAsync().Result;
                        }
                        catch (Exception e)
                        {
                            throw new CannotGetContentException(uri.ToString(), e);
                        }
                    }

                case FILE:
                    try
                    {
                        return File.ReadAllText(Uri.UnescapeDataString(uri.AbsolutePath));
                    }
                    catch (Exception e)
                    {
                        throw new CannotGetContentException(uri.ToString(), e);
                    }

                case FTP:
                    try
                    {

                    }
                    catch (Exception e)
                    {

                    }
                    break;

                default:
                    throw new FormatNotSupportedException(uri.ToString());
            }

            return string.Empty;
        }

        public string GetM3U8Content(string uri)
        {
            try
            {
                var _uri = new Uri(uri);
                return GetM3U8Content(_uri);
            }
            catch
            {
                throw;
            }
        }
    }
}
