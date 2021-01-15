using M3U8Downloader.Core;
using M3U8Downloader.Core.Events;
using M3U8Downloader.Core.Exceptions.DownloadExceptions;
using M3U8Downloader.Core.Interfaces;
using M3U8Downloader.Core.Models;
using M3U8Downloader.Core.MVVM;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace M3U8Downloader.Modules.MainModule.ViewModels
{
    internal class TaskListViewModel : ViewModelBase
    {
        private readonly IContainerProvider _provider;
        private readonly IEventAggregator _eventAggregator;

        #region Service
        private readonly IDownloadTaskManageService _taskManager;
        private readonly IDownloadService _downloadService;
        private readonly IM3U8DownloadTaskToolService _tool;
        private readonly IUriService _uriService;
        private readonly IM3U8FileContentAnalyseService _m3u8FileContentAnalyseService;
        private readonly IM3U8TaskDatabaseService _databaseService;
        private readonly IVideoComposeService _composeVideoService;
        private readonly IVideosMergeService _videosMergeService;
        #endregion

        public TaskListViewModel(IContainerProvider containerProvider)
        {

            #region Reslve
            _provider = containerProvider;
            _eventAggregator = _provider.Resolve<IEventAggregator>();
            _tool = _provider.Resolve<IM3U8DownloadTaskToolService>();
            _taskManager = _provider.Resolve<IDownloadTaskManageService>();
            _downloadService = _provider.Resolve<IDownloadService>();
            _uriService = _provider.Resolve<IUriService>();
            _m3u8FileContentAnalyseService = _provider.Resolve<IM3U8FileContentAnalyseService>();
            _composeVideoService = _provider.Resolve<IVideoComposeService>();
            _databaseService = _provider.Resolve<IM3U8TaskDatabaseService>();
            _videosMergeService = _provider.Resolve<IVideosMergeService>();
            #endregion

            TaskList = _taskManager.GetTaskList();

            #region Subscribe Events
            _eventAggregator.GetEvent<DownloadTaskListNeedAddEvent>().Subscribe(OnDownloadTaskListNeedAdd);
            _eventAggregator.GetEvent<DownloadTaskListNeedRemoveEvent>().Subscribe(OnDownloadTaskListNeedRemove);
            _eventAggregator.GetEvent<WindowSizeChangedEvent>().Subscribe(OnWindowSizeChanged);
            _eventAggregator.GetEvent<AllDownloadTasksStateNeedChangeEvent>().Subscribe(OnAllDownloadTasksStateNeedChange);
            _eventAggregator.GetEvent<FinishedDownloadTaskNeedRemoveEvent>().Subscribe(OnFinishedDownloadTaskNeedRemove);
            _eventAggregator.GetEvent<DownloadTaskStateNeedChangeEvent>().Subscribe(OnDownloadTaskStateNeedChange);
            _eventAggregator.GetEvent<DownloadTaskErrorEvent>().Subscribe(OnDownloadTaskError);
            #endregion
        }

        #region Event Hanlders

        private void OnDownloadTaskError(DownloadTaskErrorEventArgs args)
        {
            var task = args.Task;
            _taskManager.SetState(task, TaskState.ERROR);
        }

        private void OnFinishedDownloadTaskNeedRemove(FinishedDownloadTaskNeedRemoveEventArgs args)
        {
            var allFinished = (
                from task in TaskList where task.State == TaskState.FINISHED select task
                ).ToList();
            foreach (var finished in allFinished)
            {
                RemoveTask(TaskList.IndexOf(finished));
            }
        }

        private void OnAllDownloadTasksStateNeedChange(AllDownloadTasksStateNeedChangeEventArgs args)
        {
            switch (args.Mode)
            {
                case AllDownloadTasksStateNeedChangeEventArgs.NeedChangeMode.NEED_START:
                    StartAllDownloadTask();
                    break;
                case AllDownloadTasksStateNeedChangeEventArgs.NeedChangeMode.NEED_STOP:
                    StopAllDownloadTask();
                    break;
                default:
                    break;
            }
        }
        private void OnDownloadTaskStateNeedChange(DownloadTaskStateNeedChangeEventArgs args)
        {
            var task = args.Task;
            if (_tool.CanStart(task) || _tool.CanRetry(task))
            {
                if (IsTaskValidOrPublishEvent(task))
                {
                    _taskManager.SetState(task, TaskState.STARTED);
                }
            }
            if (HasNoTaskDownloading())
            {
                StartDownloadTask(task);
            }
        }

        private void OnWindowSizeChanged(WindowSizeChangedEventArgs args)
        {
            ContentHeight = args.ActualHeight - RegionSizes.MenuHeight_Size;
        }

        private void OnDownloadTaskListNeedAdd(DownloadTaskListNeedAddEventArgs args)
        {
            AddNewTasks(args.Count);
        }

        private void OnDownloadTaskListNeedRemove(DownloadTaskListNeedRemoveEventArgs args)
        {
            RemoveTask(TaskList.IndexOf(SelectedTask));
        }

        #endregion

        private void StartAllDownloadTask()
        {
            foreach (var task in TaskList)
            {
                if (_tool.CanStart(task))
                {
                    if (IsTaskValidOrPublishEvent(task))
                    {
                        _taskManager.SetState(task, TaskState.STARTED);
                    }
                }
            }
            if (HasNoTaskDownloading())
            {
                var task = GetFirstStartedButNotInDownloadTask();

                if (task is not null)
                {
                    StartDownloadTask(task);
                }
            }
        }

        /// <summary>
        /// Checks whether every arguments of the task all are valid.
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Whether they all are valid.</returns>
        private bool IsTaskValidOrPublishEvent(M3U8DownloadTask task)
        {
            bool isError = false;
            if (!_tool.IsUriValid(task))
            {
                isError = true;
                _eventAggregator.GetEvent<DownloadTaskErrorEvent>().Publish(
                    new DownloadTaskErrorEventArgs(task, DownloadTaskErrorEventArgs.ErrorType.URI)
                    );
            }
            else if (!_tool.IsTargetFolderValid(task))
            {
                isError = true;
                _eventAggregator.GetEvent<DownloadTaskErrorEvent>().Publish(
                    new DownloadTaskErrorEventArgs(task, DownloadTaskErrorEventArgs.ErrorType.TargetFolder)
                    );
            }

            if (isError)
            {
                _taskManager.SetState(task, TaskState.ERROR);
            }
            return isError;
        }

        private void StopAllDownloadTask()
        {
            foreach (var task in TaskList)
            {
                if (_tool.CanStop(task))
                {
                    _taskManager.SetState(task, TaskState.STOPPED);
                }
            }
        }

        private void DownloadNextTask()
        {
            var next = GetFirstStartedButNotInDownloadTask();

            if (next is not null)
            {
                StartDownloadTask(next);
            }
        }


        private void StartDownloadTask(M3U8DownloadTask task)
        {
            _eventAggregator.GetEvent<DownloadTaskStartedEvent>().Publish(
                new DownloadTaskStartedEventArgs(task)
            );
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Task.Run(async () =>
            {
                _taskManager.SetState(task, TaskState.DOWNLOADING);
                try
                {
                    var content = _uriService.GetM3U8Content(task.Uri);
                    AddPer();

                    var spans = _m3u8FileContentAnalyseService.GetEveryDownloadSpan(task, content);
                    AddPer();

                    _databaseService.TryAddSpans(task, spans);

                    _downloadService.DownloadDataFrom(spans, SpanDownloadExceptionHandler);
                    AddPer();

                    List<string> filesList = null;

                    filesList = _composeVideoService.ComposeVideo(task);
                    AddPer();

                    token.ThrowIfCancellationRequested();

                    _databaseService.TryRemoveSpans(task);

                    OnWaitMerge();
                    var successful = await Task.Run(() =>
                    {
                        return _videosMergeService.MergeVideo(filesList, _tool.GetOutputFileFullName(task));
                    });
                    AddPer();

                    _taskManager.SetState(task, successful ? TaskState.FINISHED : TaskState.ERROR);
                }
                catch
                {
                    OnError();
                }

                void AddPer()
                {
                    const int PER = 100 / 5;
                    task.Progress += PER;
                }

            }, token);

            void OnError()
            {
                tokenSource.Cancel();
                _taskManager.SetState(task, TaskState.ERROR);
            }

            void OnWaitMerge()
            {
                DownloadNextTask();
            }

            void SpanDownloadExceptionHandler(M3U8DownloadSpan span, SpanDownloadException e)
            {
                Console.WriteLine(e.ToString());
            }

            void OnDownloadTaskAlreadyStartedException()
            {
            }
        }

        #region Add&Remove

        /// <summary>
        /// Removes the task.
        /// <br/>
        /// NOTE:Don't call this when you iterate the <see cref="TaskList">TaskList</see> ,because the method would remove one then disorder the order .
        /// </summary>
        /// <param name="index">The index of the task.</param>
        private void RemoveTask(int index)
        {
            if (index < 0)
            {
                return;
            }
            try
            {
                var needRemove = TaskList[index];
                var needReselect = needRemove == SelectedTask && TaskList.Count > 1;
                M3U8DownloadTask reselected = null;
                if (needReselect)
                {
                    //If the task needed to remove was the last one in TaskList , it would select its previous.
                    var delta = TaskList.IndexOf(needRemove) == TaskList.Count - 1 ? -1 : 1;
                    reselected = TaskList[index + delta];
                }
                _taskManager.RemoveTask(needRemove);
                _eventAggregator.GetEvent<DownloadTaskListCountChangedEvent>().Publish(
                    new DownloadTaskListCountChangedEventArgs(TaskList.Count)
                    {
                        Mode = DownloadTaskListCountChangedEventArgs.ChangedMode.REMOVED,
                        IsChangedCountSingular = true,
                        ChangedTask = needRemove
                    });
                if (needReselect)
                {
                    SelectedTask = reselected;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                ;
            }

        }

        private void AddNewTasks(int count)
        {
            var addedSuccessfully = false;
            if (count == 1)
            {
                if (SelectedTask is not null)
                {
                    //Notify saving the download task being edited currently.
                    _eventAggregator.GetEvent<CurrentlyEditedDownloadTaskNeedBeSavedEvent>().Publish(
                       new CurrentlyEditedDownloadTaskNeedBeSavedEventArgs()
                       );
                }
                if (SelectedTask is null || !_tool.HasEmpty(TaskList))
                {
                    var newTask = _taskManager.AddNewTask();
                    addedSuccessfully = true;

                    _eventAggregator.GetEvent<DownloadTaskListCountChangedEvent>().Publish(
                    new DownloadTaskListCountChangedEventArgs(TaskList.Count)
                    {
                        Mode = DownloadTaskListCountChangedEventArgs.ChangedMode.ADDED,
                        IsChangedCountSingular = true,
                        ChangedTask = newTask
                    });
                }
            }
            else if (count > 1)
            {
                if (SelectedTask is not null)
                {
                    //Notify saving the download task being edited currently.
                    _eventAggregator.GetEvent<CurrentlyEditedDownloadTaskNeedBeSavedEvent>().Publish(
                       new CurrentlyEditedDownloadTaskNeedBeSavedEventArgs()
                       );
                }

                var added = new List<M3U8DownloadTask>(count);
                for (var i = 0; i < count; i++)
                {
                    var newTask = _taskManager.AddNewTask();
                    added.Add(newTask);
                }
                addedSuccessfully = true;
                _eventAggregator.GetEvent<DownloadTaskListCountChangedEvent>().Publish(
                    new DownloadTaskListCountChangedEventArgs(TaskList.Count)
                    {
                        Mode = DownloadTaskListCountChangedEventArgs.ChangedMode.ADDED,
                        IsChangedCountSingular = false,
                        ChangedTasks = added
                    });

            }
            if (addedSuccessfully)
            {
                SelectedTask = TaskList[0];
            }
        }

        #endregion

        private bool HasNoTaskDownloading()
        {
            foreach (var task in TaskList)
            {
                if (task.State == TaskState.DOWNLOADING)
                {
                    return false;
                }
            }
            return true;
        }

        private M3U8DownloadTask GetFirstStartedButNotInDownloadTask()
        {
            foreach (var task in TaskList)
            {
                if (_tool.IsStartedButNotInDownload(task))
                {
                    return task;
                }
            }
            return null;
        }

        private ObservableCollection<M3U8DownloadTask> _taskList;
        public ObservableCollection<M3U8DownloadTask> TaskList
        {
            get => _taskList;
            set => SetProperty(ref _taskList, value);
        }

        private M3U8DownloadTask _selectedTask;
        public M3U8DownloadTask SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (SetProperty(ref _selectedTask, value) && value is not null)
                {
                    _eventAggregator.GetEvent<CurrentlyEditedDownloadTaskNeedBeSavedEvent>().Publish(
                        new CurrentlyEditedDownloadTaskNeedBeSavedEventArgs()
                        );
                    _eventAggregator.GetEvent<DownloadTaskSelectedEvent>().Publish(
                        new DownloadTaskSelectedEventArgs(value)
                        );
                }
            }
        }

        private double _conetntHieght = RegionSizes.TaskListHeight_Size;
        public double ContentHeight
        {
            get => _conetntHieght;
            set => SetProperty(ref _conetntHieght, value);
        }
    }
}
