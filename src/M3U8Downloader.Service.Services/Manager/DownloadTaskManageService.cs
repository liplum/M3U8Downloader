using M3U8Downloader.Core.Events;
using M3U8Downloader.Core.Interfaces.Analysis;
using M3U8Downloader.Core.Interfaces.Cache;
using M3U8Downloader.Core.Interfaces.IO;
using M3U8Downloader.Core.Interfaces.Manager;
using M3U8Downloader.Core.Interfaces.Net;
using M3U8Downloader.Core.Interfaces.Tool;
using M3U8Downloader.Core.Models;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace M3U8Downloader.Service.Services.Manager
{
    public class DownloadTaskManageService : IDownloadTaskManageService
    {
        private readonly ObservableCollection<M3U8DownloadTask> _tasklist;
        private readonly IContainerProvider _provider;
        private readonly IDownloadTaskStateManageService _stateManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUriService _uriService;
        private readonly IM3U8FileContentAnalysisService _analyser;
        private readonly IM3U8TaskCacheService _taskCache;
        private readonly IDownloadService _downloader;
        private readonly IVideoComposeService _videoComposer;
        private readonly IVideosMergeService _videosMerger;
        private readonly IDownloadTaskToolService _tool;


        public DownloadTaskManageService(IContainerProvider containerProvider)
        {
            _tasklist = new ObservableCollection<M3U8DownloadTask>();

            #region Resolve
            _provider = containerProvider;
            _stateManager = _provider.Resolve<IDownloadTaskStateManageService>();
            _eventAggregator = _provider.Resolve<IEventAggregator>();
            _uriService = _provider.Resolve<IUriService>();
            _analyser = _provider.Resolve<IM3U8FileContentAnalysisService>();
            _taskCache = _provider.Resolve<IM3U8TaskCacheService>();
            _downloader = _provider.Resolve<IDownloadService>();
            _videoComposer = _provider.Resolve<IVideoComposeService>();
            _videosMerger = _provider.Resolve<IVideosMergeService>();
            _tool = _provider.Resolve<IDownloadTaskToolService>();
            #endregion
        }

        #region Discard
        /*
        /// <summary>
        /// First one is current state.
        /// <br/>
        /// Second one is previous state.
        /// </summary>
        private readonly ConcurrentDictionary<M3U8DownloadTask,> _datebase;

         public DownloadTaskManageService()
         {
             _datebase = new ConcurrentDictionary<M3U8DownloadTask, (TaskState, TaskState)>();
             _tasklist = new ObservableCollection<M3U8DownloadTask>();
         }

         public bool ReturnPreviousState(M3U8DownloadTask task)
         {
             try
             {
                 if (_datebase.ContainsKey(task))
                 {
                     (var originalCurrentState, var originalPreviouseState) = _datebase[task];
                     return SetState(task, needChange: originalPreviouseState);
                 }
                 else
                 {
                     return false;
                 }
             }
             catch
             {
                 return false;
             }
         }

         public bool SetState(M3U8DownloadTask task, TaskState needChange)
         {
             try
             {
                 if (_datebase.ContainsKey(task))
                 {
                     (var originalCurrentState, var originalPreviouseState) = _datebase[task];
                     //Cann't Set
                     if (needChange == originalCurrentState)
                         //If needChange was the same as originalCurrentState
                     {
                         return false;
                     }
                     if (originalCurrentState == TaskState.FINISHED)
                         //When the task has already Finished.
                     {
                         return false;
                     }
                     if (needChange == TaskState.STOPPED && originalCurrentState == TaskState.DOWNLOADING)
                         //During the process of downloading , it can't stop.
                     {
                         return false;
                     }
                     //
                     _datebase[task] = (needChange, originalCurrentState);
                     task.State = needChange;
                 }
                 else
                 {
                     _datebase.TryAdd(task, (needChange, task.State));
                 }

                 return true;
             }
             catch
             {
                 return false;
             }
         }
        */
        #endregion

        public bool CheckParameters(M3U8DownloadTask task)
        {
            return true;
        }

        public void StartDownload(M3U8DownloadTask task)
        {
            _stateManager.FireAsync(task, TaskTrigger.START);
        }

        public M3U8DownloadTask AddNewTask()
        {
            var task = new M3U8DownloadTask()
            {
                //TODO:
                TimeStamp = DateTime.Now,
                IsDefaultTargetFolder = true,
            };

            _stateManager.AddTask(task);
            ConfigureStates();

            _tasklist.Insert(0, task);

            return task;

            void ConfigureStates()
            {
                _stateManager.ConfigureAsyncOnEntry(task,
                    TaskState.STARTED,
                    () =>
                    {
                        return Task.Run(() =>
                        {
                            if (CheckParameters(task))
                            {
                                AddPer();
                                _stateManager.FireAsync(task, TaskTrigger.LOAD);
                            }
                            else
                            {
                                _stateManager.FireAsync(task, TaskTrigger.PARAMETER_ERROR);
                            }
                        });
                    });

                _stateManager.ConfigureAsyncOnEntry(task,
                   TaskState.LOADING,
                   () =>
                   {
                       return Task.Run(() =>
                       {
                           try
                           {
                               var content = _uriService.GetM3U8Content(task.Uri);
                               AddPer();
                               var spans = _analyser.GetEveryDownloadSpan(task, content);
                               AddPer();
                               _taskCache.TryAddSpans(task, spans);
                               AddPer();
                               _stateManager.FireAsync(task, TaskTrigger.DOWNLOAD);
                           }
                           catch (Exception)
                           {
                               _stateManager.FireAsync(task, TaskTrigger.LOAD_ERROR);
                           }
                       });
                   });

                _stateManager.ConfigureAsyncOnEntry(task,
                   TaskState.DOWNLOADING,
                   () =>
                   {
                       return Task.Run(() =>
                       {
                           _downloader.DownloadDataFromTask(task, null);
                           AddPer();
                           _stateManager.FireAsync(task, TaskTrigger.COMPOSE);
                       });
                   });


                _stateManager.ConfigureAsyncOnEntry(task,
                   TaskState.COMPOSING,
                   () =>
                   {
                       return Task.Run(() =>
                       {
                           var fileList = _videoComposer.ComposeVideo(task);
                           AddPer();
                           var isSuccessful = _videosMerger.MergeVideo(fileList, _tool.GetOutputFileFullName(task));
                           if (isSuccessful)
                           {
                               AddPer();
                               _stateManager.FireAsync(task, TaskTrigger.COMPOSE_COMPLETED);
                           }
                           else
                           {
                               _stateManager.FireAsync(task, TaskTrigger.COMPOSE_ERROR);
                           }
                       });
                   });

                _stateManager.ConfigureOnEntry(task,
                  TaskState.FINISHED,
                  () =>
                  {
                      _eventAggregator.GetEvent<DownloadTaskStateChangedEvent>().Publish(
                          new DownloadTaskStateChangedEventArgs(DownloadTaskStateChangedEventArgs.ChangeType.FINISHED)
                          {
                              Task = task
                          });
                      ;
                  });

                /*_stateManager.ConfigureOnExit(task,
                   TaskState.EDITING,
                   () =>
                   {

                   });*/
            }

            void AddPer()
            {
                const int PER = 100 / 7;
                task.Progress += PER;
            }
        }

        public ObservableCollection<M3U8DownloadTask> GetTaskList()
        {
            return _tasklist;
        }

        public bool RemoveTask(M3U8DownloadTask task)
        {
            return _tasklist.Remove(task) && _stateManager.RemoveTask(task);
        }

        public void EndEdit(M3U8DownloadTask task)
        {
            _stateManager.Fire(task, TaskTrigger.END_EDIT);
        }

        public void Edit(M3U8DownloadTask task)
        {
            _stateManager.Fire(task, TaskTrigger.EDIT);
        }
    }
}
