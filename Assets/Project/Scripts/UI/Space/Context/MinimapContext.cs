using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.Space.UI
{
    public class MinimapContext : Context, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private Texture _texture;
        private Texture _defaultTexture;
        
        [UsedImplicitly]
        public Texture Texture
        {
            get => _texture;
            set
            {
                _texture = value == null ? DefaultTexture : value;
                OnPropertyChanged();
            }
        }

        public Texture DefaultTexture
        {
            get => _defaultTexture;
            set
            {
                _defaultTexture = value;
                if (Texture == null)
                    Texture = value;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
