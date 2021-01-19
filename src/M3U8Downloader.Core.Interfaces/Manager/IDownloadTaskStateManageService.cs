using M3U8Downloader.Core.Models;
using System;
using System.Threading.Tasks;

namespace M3U8Downloader.Core.Interfaces.Manager
{
    public interface IDownloadTaskStateManageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool AddTask(M3U8DownloadTask task);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool RemoveTask(M3U8DownloadTask task);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskTrigger"></param>
        public void Fire(M3U8DownloadTask task, TaskTrigger taskTrigger);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskTrigger"></param>
        /// <returns></returns>
        public Task FireAsync(M3U8DownloadTask task, TaskTrigger taskTrigger);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskState"></param>
        /// <param name="entryAction"></param>
        public void ConfigureOnEntry(M3U8DownloadTask task, TaskState taskState, Action entryAction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskState"></param>
        /// <param name="exitAction"></param>
        public void ConfigureOnExit(M3U8DownloadTask task, TaskState taskState, Action exitAction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskState"></param>
        /// <param name="entryAction"></param>
        public void ConfigureAsyncOnEntry(M3U8DownloadTask task, TaskState taskState, Func<Task> entryAction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskState"></param>
        /// <param name="exitAction"></param>
        public void ConfigureAsyncOnExit(M3U8DownloadTask task, TaskState taskState, Func<Task> exitAction);
    }
}
