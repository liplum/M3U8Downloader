using M3U8Downloader.Core.Events;
using M3U8Downloader.Core.Interfaces.Global;
using M3U8Downloader.Core.MVVM;
using M3U8Downloader.MainModule.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;

namespace M3U8Downloader.MainModule.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IContainerProvider _provider;


        public MenuViewModel(IContainerProvider containerProvider)
        {
            _provider = containerProvider;
            _eventAggregator = _provider.Resolve<IEventAggregator>();

            AppCommand = _provider.Resolve<IApplicationCommand>();

            _eventAggregator.GetEvent<DownloadTaskListCountChangedEvent>().Subscribe(OnDownloadTaskListCountChanged);
            _eventAggregator.GetEvent<DownloadTaskStateChangedEvent>().Subscribe(OnDownloadTaskStateChanged);
        }

        private void OnDownloadTaskListCountChanged(DownloadTaskListCountChangedEventArgs args)
        {
            CanRemove = args.CurrentCount > 0;
        }
        private void OnDownloadTaskStateChanged(DownloadTaskStateChangedEventArgs args)
        {
            //coming soon...
        }

        private IApplicationCommand _appCmd;
        public IApplicationCommand AppCommand
        {
            get
            {
                return _appCmd;
            }
            set
            {
                SetProperty(ref _appCmd, value);
            }
        }

        private bool _canRemove = false;
        public bool CanRemove
        {
            get => _canRemove;
            set => SetProperty(ref _canRemove, value);
        }

        private ButtonState _startedStopedState = ButtonState.Stopped;
        public ButtonState StartedStoppedState
        {
            get => _startedStopedState;
            set => SetProperty(ref _startedStopedState, value);
        }

        #region DelegateCommand StartOrStopAllTasksCommand

        private DelegateCommand _startOrStopAllTasksCommand;
        public DelegateCommand StartOrStopAllTasksCommand =>
            _startOrStopAllTasksCommand ??= new DelegateCommand(ExecuteStartOrStopAllTasksCommand);

        void ExecuteStartOrStopAllTasksCommand()
        {
            switch (StartedStoppedState)
            {
                case ButtonState.Started:
                    StopAllTasks();
                    break;
                case ButtonState.Stopped:
                    StartAllTask();
                    break;
            }
        }

        private void StartAllTask()
        {
            _eventAggregator.GetEvent<GlobalDownloadCommandEvent>().Publish(
                new GlobalDownloadCommandEventArgs(
                    GlobalDownloadCommandEventArgs.Command.START)
                );
        }
     
        private void StopAllTasks()
        {
            _eventAggregator.GetEvent<GlobalDownloadCommandEvent>().Publish(
                new GlobalDownloadCommandEventArgs(
                    GlobalDownloadCommandEventArgs.Command.STOP)
                );
        }

        #endregion

        #region DelegateCommand AddNewTaskCommand
        private DelegateCommand _addNewTaskCommand;
        public DelegateCommand AddNewTaskCommand =>
            _addNewTaskCommand ??= new DelegateCommand(ExecuteAddNewTask, CanExecuteAddNewTask);

        private void ExecuteAddNewTask()
        {
            _eventAggregator.GetEvent<DownloadTaskListNeedAddEvent>().Publish(new DownloadTaskListNeedAddEventArgs());
        }

        private bool CanExecuteAddNewTask()
        {
            return true;
        }
        #endregion

        #region RemoveCurrentTaskCommand
        private DelegateCommand _removeCurrentTaskCommand;
        public DelegateCommand RemoveCurrentTaskCommand =>
            _removeCurrentTaskCommand ??= new DelegateCommand(ExecuteRemoveCurrentTask, CanExecuteRemoveCurrentTask).ObservesProperty(() => CanRemove);

        private void ExecuteRemoveCurrentTask()
        {
            _eventAggregator.GetEvent<DownloadTaskListNeedRemoveEvent>().Publish(new DownloadTaskListNeedRemoveEventArgs());
        }

        private bool CanExecuteRemoveCurrentTask()
        {
            return CanRemove;
        }
        #endregion
    }
}
