using M3U8Downloader.Core.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;

namespace M3U8Downloader.Shell.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private DelegateCommand<object> _windowSizeChangedCommand;
        public DelegateCommand<object> WindowSizeChangedCommand =>
            _windowSizeChangedCommand ??= new DelegateCommand<object>(ExecuteWindowSizeChangedCommand);

        private void ExecuteWindowSizeChangedCommand(object param)
        {
            var element = param as FrameworkElement;
            if (element is not null)
            {
                _eventAggregator.GetEvent<WindowSizeChangedEvent>().Publish(
                    new WindowSizeChangedEventArgs
                    {
                        ActualHeight = element.ActualHeight,
                        ActualWidth = element.ActualWidth
                    }
                    );
            }
        }

        private string _title = "Prism Application";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _detailPageDisplayed = false;
        public bool DetailPageDisplayed
        {
            get => _detailPageDisplayed;
            set => SetProperty(ref _detailPageDisplayed, value);
        }

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<DownloadTaskListCountChangedEvent>().Subscribe(OnTaskCountChanged);
        }

        private void OnTaskCountChanged(DownloadTaskListCountChangedEventArgs args)
        {
            var count = args.CurrentCount;
            DetailPageDisplayed = count > 0;
        }

    }
}
