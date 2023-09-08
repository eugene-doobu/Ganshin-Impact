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
        
        private Color _speakerColor;
        private Texture _texture;

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
        public Texture Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                OnPropertyChanged();
                
                SpeakerColor = value == null ? Color.clear : Color.white;
            }
        }

        [UsedImplicitly]
        public Color SpeakerColor
        {
            get => _speakerColor;
            set
            {
                _speakerColor = value;
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
