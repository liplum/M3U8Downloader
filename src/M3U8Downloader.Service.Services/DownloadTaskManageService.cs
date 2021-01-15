using M3U8Downloader.Core.Interfaces;
using M3U8Downloader.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace M3U8Downloader.Service.Services
{
    public class DownloadTaskManageService : IDownloadTaskManageService
    {
        /// <summary>
        /// First one is current state.
        /// <br/>
        /// Second one is previous state.
        /// </summary>
        private readonly ConcurrentDictionary<M3U8DownloadTask, (TaskState, TaskState)> _datebase;
        private readonly ObservableCollection<M3U8DownloadTask> _tasklist;

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
                    if (needChange == originalCurrentState)//If needChange was the same as originalCurrentState
                    {
                        return false;
                    }
                    if (originalCurrentState == TaskState.FINISHED)//When the task has already Finished.
                    {
                        return false;
                    }
                    if (needChange == TaskState.STOPPED && originalCurrentState == TaskState.DOWNLOADING)//During the process of downloading , it can't stop.
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
        public M3U8DownloadTask AddNewTask()
        {
            var task = new M3U8DownloadTask()
            {
                //TODO:
                TimeStamp = DateTime.Now,
                IsDefaultTargetFolder = true
            };
            SetState(task, TaskState.NOT_STARTED);
            _tasklist.Insert(0, task);

            return task;
        }

        public ObservableCollection<M3U8DownloadTask> GetTaskList()
        {
            return _tasklist;
        }

        public bool AddTask(M3U8DownloadTask task)
        {
            var res = false;
            if (!_datebase.ContainsKey(task))
            {
                res = _datebase.TryAdd(task, (task.State, task.State));
            }
            _tasklist.Add(task);
            return res;
        }

        public bool RemoveTask(M3U8DownloadTask task)
        {
            return _tasklist.Remove(task) && _datebase.TryRemove(task, out _);
        }
    }
}
