﻿using Capoala.MVVM;
using Capoala_Samples.Models;
using System;
using System.Threading.Tasks;

namespace Capoala_Samples.ViewModels
{    
    internal class SampleViewModel : NotifyPropertyChangesBase
    {
        public SampleViewModel()
        {
            SubView = MessageSubView;

            SaveCommand = new CommandRelay(async () => await SaveAsync(), () => !IsWorkInProgress);
        }

        private MessageModel MessageSubView { get; } = new MessageModel() { Message = "Hello, World!" };
        private ProgressReporter SaveProgressSubView { get; } = new ProgressReporter() { CurrentProgressComplete = 0, Status = null };

        private bool _isWorkInProgress = false;
        public bool IsWorkInProgress
        {
            get => _isWorkInProgress;
            set => SetAndNotify(ref _isWorkInProgress, value);
        }

        private INotifyPropertyChanges _subView;
        public INotifyPropertyChanges SubView
        {
            get => _subView;
            set => SetAndNotify(ref _subView, value);
        }

        [CanExecuteDependentOn(nameof(IsWorkInProgress))]
        public CommandRelay SaveCommand { get; }

        private async Task SaveAsync()
        {
            IsWorkInProgress = true;
            SubView = SaveProgressSubView;

            for (var i = 1; i <= 10; i++)
            {
                SaveProgressSubView.CurrentProgressComplete = i * 10;
                SaveProgressSubView.Status = $"Saving... {SaveProgressSubView.CurrentProgressComplete}%";
                await Task.Delay(TimeSpan.FromMilliseconds(750));
            }

            SaveProgressSubView.CurrentProgressComplete = 100;
            SaveProgressSubView.Status = "Complete!";
            await Task.Delay(TimeSpan.FromSeconds(2));
            SubView = MessageSubView;
            SaveProgressSubView.CurrentProgressComplete = 0;
            SaveProgressSubView.Status = null;
            IsWorkInProgress = false;
        }
    }
}
