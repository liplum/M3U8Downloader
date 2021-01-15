using M3U8Downloader.Core.Events;
using M3U8Downloader.Core.Interfaces;
using M3U8Downloader.Core.Models;
using M3U8Downloader.Core.MVVM;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using WPFLocalizeExtension.Engine;

namespace M3U8Downloader.Modules.MainModule.ViewModels
{
    internal class DetailPageViewModel : ViewModelBase
    {
        private readonly IContainerProvider _provider;
        private readonly IEventAggregator _eventAggregator;
        private readonly IM3U8DownloadTaskToolService _tool;
        private readonly IDownloadTaskManageService _taskManager;
        private readonly ILocalizeHelperService _locHelper;

        private string _resourcesHead;
        private string ResourcesHead
        {
            get => _resourcesHead;
            init
            {
                _resourcesHead = Assembly.GetExecutingAssembly().GetName().Name;
            }
        }

        private M3U8DownloadTask _currentTask;
        public M3U8DownloadTask CurrentTask
        {
            get => _currentTask;
            set => SetProperty(ref _currentTask, value);
        }

        public DetailPageViewModel(IContainerProvider containerProvider)
        {
            _provider = containerProvider;
            _eventAggregator = _provider.Resolve<IEventAggregator>();
            _tool = _provider.Resolve<IM3U8DownloadTaskToolService>();
            _taskManager = _provider.Resolve<IDownloadTaskManageService>();
            _locHelper = _provider.Resolve<ILocalizeHelperService>();

            _eventAggregator.GetEvent<DownloadTaskSelectedEvent>().Subscribe(OnDownloadTaskSelected);
            _eventAggregator.GetEvent<CurrentlyEditedDownloadTaskNeedBeSavedEvent>().Subscribe(OnCurrentlyEditedDownloadTaskNeedBeSaved);
            LocalizeDictionary.Instance.PropertyChanged += OnLanguageChanged;
        }

        private void OnCurrentTaskChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentTask.State):
                    OnCurrentTaskStateChanged();
                    break;
            }
        }

        private void OnCurrentTaskStateChanged()
        {
            CanEdit = _tool.CanEdit(CurrentTask);
        }

        private void OnLanguageChanged(object sender, PropertyChangedEventArgs e)
        {
            DefaultTargetFolderText = GetLocString(key: nameof(Properties.Resources.DetailPage_DefaultTarget));
        }

        private string GetLocString(string key)
        {
            return _locHelper.GetLocalizedString(assemblyName: ResourcesHead, key);
        }

        private void OnCurrentlyEditedDownloadTaskNeedBeSaved(CurrentlyEditedDownloadTaskNeedBeSavedEventArgs args)
        {
            SaveCurrentTask();
        }

        private void OnDownloadTaskSelected(DownloadTaskSelectedEventArgs args)
        {
            PreviousTaskHandle();
            NewTaskHandle();

            void PreviousTaskHandle()
            {
                if (CurrentTask is not null)
                {
                    CurrentTask.PropertyChanged -= OnCurrentTaskChanged;
                    _taskManager.ReturnPreviousState(CurrentTask);
                }
            }

            void NewTaskHandle()
            {
                var selected = args.SelectedDownloadTask;
                if (selected is not null)
                {
                    CurrentTask = selected;
                    CurrentTask.PropertyChanged += OnCurrentTaskChanged;
                    OnCurrentTaskStateChanged();
                    Address = CurrentTask.Uri;
                    FileName = CurrentTask.FileName;
                }
            }
        }

        private void SaveCurrentTask()
        {
            if (CurrentTask is not null)
            {
                ReflectOnDownloadTask();
                _taskManager.ReturnPreviousState(CurrentTask);
            }

            void ReflectOnDownloadTask()
            {
                CurrentTask.Uri = Address;
                CurrentTask.FileName = FileName;
            }
        }

        /// <summary>
        /// 打开选择文件夹窗口
        /// </summary>
        /// <param name="Caption">窗口标题</param>
        /// <returns></returns>
        private static DirectoryInfo SelectFolder(string Caption)
        {
            DirectoryInfo target = null;

            using var dialog = new CommonOpenFileDialog(Caption)
            {
                IsFolderPicker = true,
            };

            var mode = dialog.ShowDialog();

            if (mode == CommonFileDialogResult.Ok)
            {
                var name = dialog.FileName;

                target = new DirectoryInfo(name);

            }
            return target;
        }
        #region RecordDetailsCommand
        private DelegateCommand _recordDetailsCommand;
        public DelegateCommand RecordDetailsCommand =>
            _recordDetailsCommand ??= new DelegateCommand(ExecuteRecordDetailsCommand);

        void ExecuteRecordDetailsCommand()
        {
            SaveCurrentTask();
        }
        #endregion
        #region SaveCommand
        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand).ObservesProperty(() => CurrentTask.State);

        private void ExecuteSaveCommand()
        {
            SaveCurrentTask();
        }

        private bool CanExecuteSaveCommand()
        {
            if (CurrentTask is null)
            {
                return false;
            }
            return _tool.CanSave(CurrentTask);
        }
        #endregion

        #region SelectOrOpenTargetFolderCommand
        private DelegateCommand _selectOrOpenTargetFolderCommand;
        public DelegateCommand SelectOrOpenTargetFolderCommand =>
            _selectOrOpenTargetFolderCommand ??= new DelegateCommand(ExecuteSelectOrOpenTargetFolder, CanExecuteSelectOrOpenTargetFolder);

        private void ExecuteSelectOrOpenTargetFolder()
        {
            switch (CurrentTask.State)
            {
                case TaskState.NOT_STARTED:
                case TaskState.EDITING:
                case TaskState.STOPPED:
                    {
                        var folder = SelectFolder("Select Folder");

                        if (folder is not null)
                        {
                            CurrentTask.TargetFolder = folder.FullName;
                            CurrentTask.IsDefaultTargetFolder = false;
                            SaveCurrentTask();
                        }
                    }
                    break;
                default:
                    {
                        Process.Start("explorer.exe", CurrentTask.TargetFolder);
                    }
                    break;
            }
        }

        private bool CanExecuteSelectOrOpenTargetFolder()
        {
            return true;
        }
        #endregion

        #region StartCommand
        private DelegateCommand _startOrRetryCommand;
        public DelegateCommand StartOrRetryCommand =>
            _startOrRetryCommand ??= new DelegateCommand(ExecuteStartOrRetryCommand, CanExecuteStartOrRetryCommand).ObservesProperty(() => CurrentTask.State);

        private void ExecuteStartOrRetryCommand()
        {
            SaveCurrentTask();
            _eventAggregator.GetEvent<DownloadTaskStateNeedChangeEvent>().Publish(
                new DownloadTaskStateNeedChangeEventArgs(CurrentTask, DownloadTaskStateNeedChangeEventArgs.NeedChangeMode.NEED_START)
                );
        }

        private bool CanExecuteStartOrRetryCommand()
        {
            if (CurrentTask is null)
            {
                return false;
            }

            return _tool.CanStart(CurrentTask) || _tool.CanRetry(CurrentTask);
        }
        #endregion

        #region StopCommand
        private DelegateCommand _stopCommand;
        public DelegateCommand StopCommand =>
            _stopCommand ??= new DelegateCommand(ExecuteStopCommand, CanExecuteStopCommand).ObservesProperty(() => CurrentTask.State);

        private void ExecuteStopCommand()
        {
            _taskManager.SetState(CurrentTask, TaskState.STOPPED);
        }

        private bool CanExecuteStopCommand()
        {
            if (CurrentTask is null)
            {
                return false;
            }
            return _tool.CanStop(CurrentTask);
        }
        #endregion

        private string _fileName = string.Empty;
        public string FileName
        {
            get => _fileName;
            set
            {
                if (SetProperty(ref _fileName, value))
                {
                    if (string.IsNullOrEmpty(value) && CurrentTask is not null && _tool.CanEdit(CurrentTask))
                    {
                        _taskManager.SetState(CurrentTask, TaskState.EDITING);
                    }
                }
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get => _address;
            set
            {
                if (SetProperty(ref _address, value))
                {
                    if (string.IsNullOrEmpty(value) && CurrentTask is not null && _tool.CanEdit(CurrentTask))
                    {
                        _taskManager.SetState(CurrentTask, TaskState.EDITING);
                    }
                }
            }
        }

        private bool _canEidt;
        public bool CanEdit
        {
            get
            {
                return _canEidt;
            }
            set
            {
                SetProperty(ref _canEidt, value);
            }
        }

        private string _defaultTargetFolderText;
        public string DefaultTargetFolderText
        {
            get
            {
                return _defaultTargetFolderText;
            }
            set
            {
                SetProperty(ref _defaultTargetFolderText, value);
            }
        }
    }
}
