using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.Space.UI
{
    [UsedImplicitly]
    public class DialogueContext : Context, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _content;

        private bool _isOnNpcTexture;
        private bool _isOnPlayerTexture;
        
        private Texture _npcTexture;
        private Texture _playerTexture;

#region Properties
        [UsedImplicitly]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public bool IsOnNpcTexture
        {
            get => _isOnNpcTexture;
            set
            {
                _isOnNpcTexture = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public bool IsOnPlayerTexture
        {
            get => _isOnPlayerTexture;
            set
            {
                _isOnPlayerTexture = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public Texture NpcTexture
        {
            get => _npcTexture;
            set
            {
                _npcTexture = value;
                OnPropertyChanged();
            }
        }
        
        [UsedImplicitly]
        public Texture PlayerTexture
        {
            get => _playerTexture;
            set
            {
                _playerTexture = value;
                OnPropertyChanged();
            }
        }
#endregion Properties
        
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
