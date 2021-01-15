using Prism.Mvvm;
using System;

namespace M3U8Downloader.Core.Models
{
    public enum TaskState
    {
        /// <summary>
        /// It hasn't started yet
        /// </summary>
        NOT_STARTED = 0,
        /// <summary>
        /// It've already started but not is downloading.
        /// </summary>
        STARTED = 1,
        /// <summary>
        /// It suspended and will be skipped the next download in the list of task.
        /// </summary>
        STOPPED = 2,
        /// <summary>
        /// It begun downloading but did not finish.
        /// </summary>
        DOWNLOADING = 3,
        /// <summary>
        /// It finished.
        /// </summary>
        FINISHED = 4,
        /// <summary>
        /// There's something wrong with it.
        /// </summary>
        ERROR = 5,
        /// <summary>
        /// It's editing so that you can't start it.
        /// </summary>
        EDITING = 6,
    }
    public class M3U8DownloadTask : BindableBase
    {
        private string _uri = string.Empty;
        public string Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }
        private string _fileName = string.Empty;
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private string _targetFolder = string.Empty;
        public string TargetFolder
        {
            get => _targetFolder;
            set => SetProperty(ref _targetFolder, value);
        }

        private bool _isDefaultTargetFolder = true;
        public bool IsDefaultTargetFolder
        {
            get => _isDefaultTargetFolder;
            set => SetProperty(ref _isDefaultTargetFolder, value);
        }

        private TaskState _state;
        public TaskState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        private int _porgress = 0;
        /// <summary>
        /// The max value is 100 and min one is 0.0.
        /// </summary>
        public int Progress
        {
            get => _porgress;
            set => SetProperty(ref _porgress, value);
        }

        private DateTime _timeStamp;
        public DateTime TimeStamp
        {
            get => _timeStamp;
            set => SetProperty(ref _timeStamp, value);
        }
    }
}
