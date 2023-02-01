using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.UI
{
    public class LoadingSceneDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private string _loadingText;
        private float  _progress;
        
        [UsedImplicitly]
        public string LoadingText
        {
            get => _loadingText;
            set
            {
                _loadingText = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    public class UIRootLoadingScene : GlobalUIRootBase
    {
        public LoadingSceneDataContext LoadingSceneDataContext => 
            DataContext as LoadingSceneDataContext;
        
        protected override INotifyPropertyChanged InitializeDataContext() =>
            new LoadingSceneDataContext();

        public void Update()
        {
            if(Input.GetKeyDown("]"))
            {
                LoadingSceneDataContext.Progress += 0.1f;
            }
            else if (Input.GetKeyDown("["))
            {
                LoadingSceneDataContext.Progress -= 0.1f;
            }
        }

        public override void ClearContextData()
        {
            var context = LoadingSceneDataContext;
            context.LoadingText = string.Empty;
            context.Progress    = 0;
        }
    }
}
