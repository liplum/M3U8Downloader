using System;

namespace M3U8Downloader.Core.Exceptions
{
    namespace UriExceptions
    {

        [Serializable]
        public class FormatNotSupportedException : Exception
        {
            public FormatNotSupportedException()
            {
            }
            public FormatNotSupportedException(string message) : base(message) { }
            public FormatNotSupportedException(string message, System.Exception inner) : base(message, inner) { }
            protected FormatNotSupportedException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }


        [Serializable]
        public class CannotGetContentException : Exception
        {
            public CannotGetContentException()
            {
            }
            public CannotGetContentException(string message) : base(message) { }
            public CannotGetContentException(string message, Exception inner) : base(message, inner) { }
            protected CannotGetContentException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}
