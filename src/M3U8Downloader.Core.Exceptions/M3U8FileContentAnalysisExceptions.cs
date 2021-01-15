using System;

namespace M3U8Downloader.Core.Exceptions
{
    namespace M3U8FileContentAnalysisExceptions
    {
        [Serializable]
        public class IsNotM3U8FileException : Exception
        {
            public IsNotM3U8FileException()
            {
            }
            public IsNotM3U8FileException(string message) : base(message) { }
            public IsNotM3U8FileException(string message, Exception inner) : base(message, inner) { }
            protected IsNotM3U8FileException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}
