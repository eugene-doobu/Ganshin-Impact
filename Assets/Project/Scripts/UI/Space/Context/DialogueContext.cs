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
        private string _content;

        private string _name;

        private Color                            _speakerColor;
        private Sprite                           _sprite;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
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
    }
}