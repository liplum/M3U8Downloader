using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using M3U8Downloader.Core.Events;
using M3U8Downloader.Core.Interfaces.Manager;
using M3U8Downloader.Core.Interfaces.Tool;
using M3U8Downloader.Core.Models;
using M3U8Downloader.Core.MVVM;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using WPFLocalizeExtension.Engine;

namespace M3U8Downloader.MainModule.ViewModels
{
    internal class DetailPageViewModel : ViewModelBase
    {
        private readonly IContainerProvider _provider;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDownloadTaskToolService _tool;
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

        private string _saveButtonContent;
        private string _editButtonContent;


        private string _saveOrEditButtonContent;
        public string SaveOrEditButtonContent
        {
            get => _saveOrEditButtonContent;
            set => SetProperty(ref _saveOrEditButtonContent, value);
        }


        public enum SaveOrEditAccessibilityEnum
        {
            CanSave, CanEdit, None
        }


        private SaveOrEditAccessibilityEnum _saveOrEditState = SaveOrEditAccessibilityEnum.CanSave;
        public SaveOrEditAccessibilityEnum SaveOrEditAccessibility
        {
            get => _saveOrEditState;
            set
            {
                if (SetProperty(ref _saveOrEditState, value))
                {
                    switch (_saveOrEditState)
                    {
                        case SaveOrEditAccessibilityEnum.CanSave:
                        case SaveOrEditAccessibilityEnum.None:
                            SaveOrEditButtonContent = _saveButtonContent;
                            break;
                        case SaveOrEditAccessibilityEnum.CanEdit:
                            SaveOrEditButtonContent = _editButtonContent;
                            break;

                    }
                }
            }
        }

        public DetailPageViewModel(IContainerProvider containerProvider)
        {
            _provider = containerProvider;
            _eventAggregator = _provider.Resolve<IEventAggregator>();
            _tool = _provider.Resolve<IDownloadTaskToolService>();
            _taskManager = _provider.Resolve<IDownloadTaskManageService>();
            _locHelper = _provider.Resolve<ILocalizeHelperService>();

            _eventAggregator.GetEvent<DownloadTaskSelectedEvent>().Subscribe(OnDownloadTaskSelected);
            _eventAggregator.GetEvent<DownloadTaskActionEvent>().Subscribe(OnDownloadTaskActed);
            _eventAggregator.GetEvent<DownloadTaskListCountChangedEvent>().Subscribe(OnDownloadTaskListCountChanged);

            LocalizeDictionary.Instance.PropertyChanged += OnLanguageChanged;
        }

        private void OnDownloadTaskListCountChanged(DownloadTaskListCountChangedEventArgs args)
        {
            if (args.Type == DownloadTaskListCountChangedEventArgs.ChangedType.REMOVED && args.ChangedTask.Equals(CurrentTask))
            {
                CurrentTask = null;
            }
        }

        private void OnDownloadTaskActed(DownloadTaskActionEventArgs obj)
        {
            //throw new System.NotImplementedException();
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
            CanEdit = CurrentTask.State == TaskState.EDITING;
        }

        private void OnLanguageChanged(object sender, PropertyChangedEventArgs e)
        {
            DefaultTargetFolderText = GetLocString(key: nameof(Properties.Resources.DetailPage_DefaultTarget));
            _saveButtonContent = GetLocString(key: nameof(Properties.Resources.DetailPage_Save_Button));
            _editButtonContent = GetLocString(key: nameof(Properties.Resources.DetailPage_Edit_Button));
        }

        private string GetLocString(string key)
        {
            return _locHelper.GetLocalizedString(assemblyName: ResourcesHead, key);
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
                    _taskManager.EndEdit(CurrentTask);
                }
            }

            void NewTaskHandle()
            {
                var selected = args.SelectedDownloadTask;
                if (selected is not null)
                {
                    CurrentTask = selected;
                    CurrentTask.PropertyChanged += OnCurrentTaskChanged;
                    _taskManager.Edit(CurrentTask);
                    ChangeSaveAndEditButtonContent();
                }

                void ChangeSaveAndEditButtonContent()
                {
                    switch (selected.State)
                    {
                        case TaskState.NOT_STARTED:
                        case TaskState.ERROR:
                            SaveOrEditAccessibility = SaveOrEditAccessibilityEnum.CanEdit;
                            break;
                        case TaskState.EDITING:
                            SaveOrEditAccessibility = SaveOrEditAccessibilityEnum.CanSave;
                            break;
                        default:
                            SaveOrEditAccessibility = SaveOrEditAccessibilityEnum.None;
                            break;
                    }
                }
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
                    {
                        var folder = SelectFolder("Select Folder");

                        if (folder is not null)
                        {
                            CurrentTask.TargetFolder = folder.FullName;
                            CurrentTask.IsDefaultTargetFolder = false;
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
            _eventAggregator.GetEvent<DownloadTaskActionEvent>().Publish(
                new DownloadTaskActionEventArgs(CurrentTask, DownloadTaskActionEventArgs.Action.NEED_START)
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

        #region SaveOrEditCommand
        private DelegateCommand _saveOrEditCommand;
        public DelegateCommand SaveOrEditCommand =>
            _saveOrEditCommand ??= new DelegateCommand(ExecuteSaveOrEditCommand, CanExecuteSaveOrEditCommand)
            .ObservesProperty(() => SaveOrEditAccessibility);

        void ExecuteSaveOrEditCommand()
        {
            switch (SaveOrEditAccessibility)
            {
                case SaveOrEditAccessibilityEnum.CanSave:
                    _taskManager.EndEdit(CurrentTask);
                    break;
                case SaveOrEditAccessibilityEnum.CanEdit:
                    _taskManager.Edit(CurrentTask);
                    break;
            }
        }

        bool CanExecuteSaveOrEditCommand()
        {
            return SaveOrEditAccessibility != SaveOrEditAccessibilityEnum.None;
        }

        #endregion

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
