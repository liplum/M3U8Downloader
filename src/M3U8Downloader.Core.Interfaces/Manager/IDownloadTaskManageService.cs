﻿using M3U8Downloader.Core.Models;
using System.Collections.ObjectModel;

namespace M3U8Downloader.Core.Interfaces.Manager
{
    public interface IDownloadTaskManageService
    {
        /// <summary>
        /// Generates a M3U8 Download Task initialized by the local culture and insert it into the front of list.
        /// </summary>
        /// <returns></returns>
        public M3U8DownloadTask AddNewTask();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool CheckParameters(M3U8DownloadTask task);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void StartDownload(M3U8DownloadTask task);

        public void EndEdit(M3U8DownloadTask task);
        public void Edit(M3U8DownloadTask task);

        //public bool SetState(M3U8DownloadTask task, TaskState needChange);

        //public bool ReturnPreviousState(M3U8DownloadTask task);

        /// <summary>
        /// Returns a list of tasks.
        /// <br/>Mustn't directly add or remove a task in the list , instead , add or remove it by <see cref="AddNewTask"/> and <see cref="RemoveTask(M3U8DownloadTask)"/>
        /// </summary>
        /// <returns>A list of tasks</returns>
        public ObservableCollection<M3U8DownloadTask> GetTaskList();

        /// <summary>
        /// Add a task into the list.
        /// </summary>
        /// <returns></returns>
        //public bool AddTask(M3U8DownloadTask task);
        public bool RemoveTask(M3U8DownloadTask task);
    }
}
