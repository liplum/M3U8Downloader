using Prism.Mvvm;

namespace M3U8Downloader.Core.Models
{
    public enum SpanSate
    {
        /// <summary>
        /// It hasn't started.
        /// </summary>
        WAITING,
        /// <summary>
        /// It's downloading.
        /// </summary>
        DOWNLOADING,
        /// <summary>
        /// There's something wrong with it.
        /// </summary>
        ERROR,
        /// <summary>
        /// It finished.
        /// </summary>
        FINISHED
    }
    public class M3U8DownloadSpan : BindableBase
    {
        private string _uri = string.Empty;
        public string Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }

        private int _number;
        /// <summary>
        /// Starts With 1 and then increases by 1
        /// </summary>
        public int Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
        private SpanSate _state = SpanSate.WAITING;
        public SpanSate State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }
    }
}
