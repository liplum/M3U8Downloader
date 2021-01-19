using M3U8Downloader.Core.Interfaces.Manager;
using M3U8Downloader.Core.Models;
using Stateless;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace M3U8Downloader.Service.Services.Manager
{
    public class DownloadTaskStateMachineService : IDownloadTaskStateManageService
    {
        private readonly ConcurrentDictionary<M3U8DownloadTask, StateMachine<TaskState, TaskTrigger>> _datebase;

        public DownloadTaskStateMachineService()
        {
            _datebase = new ConcurrentDictionary<M3U8DownloadTask, StateMachine<TaskState, TaskTrigger>>();
        }

        public bool AddTask(M3U8DownloadTask task)
        {
            var sm = new StateMachine<TaskState, TaskTrigger>(
                () => task.State,
                s => task.State = s
                );


            sm.Configure(TaskState.NOT_STARTED)
                .Permit(TaskTrigger.START, TaskState.STARTED)
                .Permit(TaskTrigger.EDIT, TaskState.EDITING)
                ;

            sm.Configure(TaskState.STARTED)
                .Permit(TaskTrigger.LOAD, TaskState.LOADING)
                .Permit(TaskTrigger.PARAMETER_ERROR, TaskState.ERROR)
                ;

            sm.Configure(TaskState.LOADING)
                .Permit(TaskTrigger.DOWNLOAD, TaskState.DOWNLOADING)
                .Permit(TaskTrigger.LOAD_ERROR, TaskState.ERROR)
                ;

            sm.Configure(TaskState.DOWNLOADING)
                .Permit(TaskTrigger.COMPOSE, TaskState.COMPOSING)
                .Permit(TaskTrigger.DOWNLOAD_ERROR, TaskState.ERROR)
                ;

            sm.Configure(TaskState.COMPOSING)
                .Permit(TaskTrigger.COMPOSE_COMPLETED, TaskState.FINISHED)
                .Permit(TaskTrigger.COMPOSE_ERROR, TaskState.ERROR)
                ;

            sm.Configure(TaskState.FINISHED)
                ;

            sm.Configure(TaskState.ERROR)
                .Permit(TaskTrigger.EDIT, TaskState.EDITING)
                ;

            sm.Configure(TaskState.EDITING)
                .Permit(TaskTrigger.END_EDIT, TaskState.NOT_STARTED)
                .Permit(TaskTrigger.START, TaskState.STARTED)
                ;

            return _datebase.TryAdd(task, sm);
        }

        public void ConfigureOnEntry(M3U8DownloadTask task, TaskState taskState, Action entryAction)
        {
            if (_datebase.ContainsKey(task))
            {
                _datebase[task].Configure(taskState)
                    .OnEntry(entryAction);
            }
        }

        public void ConfigureOnExit(M3U8DownloadTask task, TaskState taskState, Action exitAction)
        {
            if (_datebase.ContainsKey(task))
            {
                _datebase[task].Configure(taskState)
                    .OnExit(exitAction);
            }
        }

        public void ConfigureAsyncOnEntry(M3U8DownloadTask task, TaskState taskState, Func<Task> entryAction)
        {
            if (_datebase.ContainsKey(task))
            {
                _datebase[task].Configure(taskState)
                    .OnEntryAsync(entryAction);
            }
        }

        public void ConfigureAsyncOnExit(M3U8DownloadTask task, TaskState taskState, Func<Task> exitAction)
        {
            if (_datebase.ContainsKey(task))
            {
                _datebase[task].Configure(taskState)
                    .OnExitAsync(exitAction);
            }
        }

        public void Fire(M3U8DownloadTask task, TaskTrigger taskTrigger)
        {
            _datebase[task].Fire(taskTrigger);
        }

        public async Task FireAsync(M3U8DownloadTask task, TaskTrigger taskTrigger)
        {
            await _datebase[task].FireAsync(taskTrigger);
        }

        public bool RemoveTask(M3U8DownloadTask task)
        {
            return _datebase.TryRemove(task, out _);
        }
    }
}
