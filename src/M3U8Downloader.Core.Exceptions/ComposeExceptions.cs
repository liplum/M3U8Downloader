using System;

namespace M3U8Downloader.Core.Exceptions
{
    namespace ComposeExceptions
    {

        [Serializable]
        public class VideoSpansExportException : Exception
        {
            public VideoSpansExportException()
            {
            }
            public VideoSpansExportException(string message) : base(message) { }
            public VideoSpansExportException(string message, System.Exception inner) : base(message, inner) { }
            protected VideoSpansExportException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }


        [Serializable]
        public class VideoComposeException : Exception
        {
            public VideoComposeException()
            {
            }
            public VideoComposeException(string message) : base(message) { }
            public VideoComposeException(string message, Exception inner) : base(message, inner) { }
            protected VideoComposeException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

    }
}
