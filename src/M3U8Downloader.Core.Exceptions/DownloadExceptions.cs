using System;

namespace M3U8Downloader.Core.Exceptions
{
    namespace DownloadExceptions
    {

        [Serializable]
        public class SpanDownloadException : Exception
        {
            public SpanDownloadException()
            {
            }
            public SpanDownloadException(string message) : base(message) { }
            public SpanDownloadException(string message, Exception inner) : base(message, inner) { }
            protected SpanDownloadException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }


        [Serializable]
        public class TargetFolderCreatException : Exception
        {
            public TargetFolderCreatException()
            {
            }
            public TargetFolderCreatException(string message) : base(message) { }
            public TargetFolderCreatException(string message, Exception inner) : base(message, inner) { }
            protected TargetFolderCreatException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}
