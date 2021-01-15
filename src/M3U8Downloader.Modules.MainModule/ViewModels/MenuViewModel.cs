using M3U8Downloader.Core.Events;
using M3U8Downloader.Core.MVVM;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;

namespace M3U8Downloader.Modules.MainModule.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IContainerProvider _provider;

        public MenuViewModel(IContainerProvider containerProvider)
        {
            _provider = containerProvider;
            _eventAggregator = _provider.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<DownloadTaskListCountChangedEvent>().Subscribe(OnDownloadTaskListCountChanged);
            _eventAggregator.GetEvent<DownloadTaskStateChangedEvent>().Subscribe(OnDownloadTaskStateChanged);
        }

        private void OnDownloadTaskListCountChanged(DownloadTaskListCountChangedEventArgs args)
        {
            CanStart = CanStop = CanRemove = args.CurrentCount > 0;
        }
        private void OnDownloadTaskStateChanged(DownloadTaskStateChangedEventArgs args)
        {
            //coming soon...
        }

        private bool _canStart = false;
        public bool CanStart
        {
            get => _canStart;
            set => SetProperty(ref _canStart, value);
        }

        private bool _canStop = false;
        public bool CanStop
        {
            get => _canStop;
            set => SetProperty(ref _canStop, value);
        }

        private bool _canRemove = false;
        public bool CanRemove
        {
            get => _canRemove;
            set => SetProperty(ref _canRemove, value);
        }

        #region DelegateCommand StartAllTasksCommand
        private DelegateCommand _startAllTasksCommand;
        public DelegateCommand StartAllTasksCommand =>
            _startAllTasksCommand ??= new DelegateCommand(ExecuteStartAllTask, CanExecuteStartAllTask).ObservesCanExecute(() => CanStart);

        private void ExecuteStartAllTask()
        {
            _eventAggregator.GetEvent<AllDownloadTasksStateNeedChangeEvent>().Publish(
                new AllDownloadTasksStateNeedChangeEventArgs(
                    AllDownloadTasksStateNeedChangeEventArgs.NeedChangeMode.NEED_START)
                );
        }

        private bool CanExecuteStartAllTask()
        {
            return CanStart;
        }
        #endregion

        #region DelegateCommand StopAllTasksCommand
        private DelegateCommand _stopAllTasksCommand;
        public DelegateCommand StopAllTasksCommand =>
            _stopAllTasksCommand ??= new DelegateCommand(ExecuteStop, CanExecuteStop).ObservesCanExecute(() => CanStop);

        private void ExecuteStop()
        {
            _eventAggregator.GetEvent<AllDownloadTasksStateNeedChangeEvent>().Publish(
                new AllDownloadTasksStateNeedChangeEventArgs(
                    AllDownloadTasksStateNeedChangeEventArgs.NeedChangeMode.NEED_STOP)
                );
        }

        private bool CanExecuteStop()
        {
            return CanStop;
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

        #region DelegateCommand OpenSettingCommand
        private DelegateCommand _openSettingCommand;
        public DelegateCommand OpenSettingCommand =>
            _openSettingCommand ??= new DelegateCommand(ExecuteSetting, CanExecuteSetting);

        private void ExecuteSetting()
        {

        }

        private bool CanExecuteSetting()
        {
            return true;
        }
        #endregion
    }
}
