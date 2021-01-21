﻿using M3U8Downloader.Core.Interfaces.Global;
using M3U8Downloader.Core.MVVM;
using Prism.Commands;
using Prism.Ioc;

namespace M3U8Downloader.Shell.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IContainerProvider _provider;
        private readonly IApplicationCommand _appCmd;

        public SettingsViewModel(IContainerProvider containerProvider)
        {
            _provider = containerProvider; 
            _appCmd = _provider.Resolve<IApplicationCommand>();
            _appCmd.OpenSettingsCommand.RegisterCommand(OpenSettingsCommand);
        }

        private DelegateCommand _openSettingsCommand;
        public DelegateCommand OpenSettingsCommand =>
            _openSettingsCommand ??= new DelegateCommand(ExecuteOpenSettingsCommand, CanExecuteOpenSettingsCommand);

        void ExecuteOpenSettingsCommand()
        {
            IsOpen = !IsOpen;
        }

        bool CanExecuteOpenSettingsCommand()
        {
            return true;
        }

        private bool _isOpen = false;
        public bool IsOpen
        {
            get
            {
                return _isOpen;
            }
            set
            {
                SetProperty(ref _isOpen, value);
            }
        }

    }
}
