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
        
        [UsedImplicitly]
        public Texture Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
