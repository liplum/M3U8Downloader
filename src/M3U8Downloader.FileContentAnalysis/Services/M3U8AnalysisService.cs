using M3U8Downloader.Core.Exceptions.M3U8FileContentAnalysisExceptions;
using M3U8Downloader.Core.Interfaces.Analysis;
using M3U8Downloader.Core.Models;
using System;
using System.Collections.Generic;

namespace M3U8Downloader.FileContentAnalysis.Services
{
    public class M3U8AnalysisService : IM3U8FileContentAnalysisService
    {
        private const string HEAD = "#EXTM3U";
        private const string INF = "#EXTINF";
        private const string END = "#EXT-X-ENDLIST";

        private const string HTTP = "http";
        private const string HTTPS = "https";

        private const string END_OF_LINE = "\r\n";

        public IEnumerable<M3U8DownloadSpan> GetEveryDownloadSpan(M3U8DownloadTask task, string m3u8FileContent)
        {
            string lineHead = null;

            var allLines = m3u8FileContent.Split(END_OF_LINE);
            if (!allLines[0].Contains(HEAD))
            {
                throw new IsNotM3U8FileException(m3u8FileContent);
            }

            var res = new LinkedList<M3U8DownloadSpan>();

            for (int index = 1, currentNumber = 1; index < allLines.Length; index++)
            {
                var line = allLines[index];
                if (line.Contains(END))
                {
                    break;
                }
                if (line.StartsWith('#'))
                {
                    continue;
                }

                var IsRelativePath = !(line.StartsWith(HTTP) || line.StartsWith(HTTPS));

                if (IsRelativePath)
                {
                    if (lineHead is null)
                    {
                        var downloadUri = new Uri(task.Uri);
                        lineHead = $"{downloadUri.Scheme}://{downloadUri.Host}{string.Join("", downloadUri.Segments[0..^1])}";
                    }
                    line = $"{lineHead}{line}";
                }

                res.AddLast(
                    new LinkedListNode<M3U8DownloadSpan>(
                        new M3U8DownloadSpan()
                        {
                            Number = currentNumber++,
                            Uri = line
                        }
                        )
                    );
            }
            return res;
        }
    }
}
