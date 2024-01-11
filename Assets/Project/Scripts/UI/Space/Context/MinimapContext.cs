using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.Space.UI
{
    public class MinimapContext : Context, INotifyPropertyChanged
    {
        private Texture _defaultTexture;

        private Texture _texture;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}