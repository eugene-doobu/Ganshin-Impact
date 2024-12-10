using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.UI.Space
{
    [UsedImplicitly]
    public class DialogueContext : GanContext
    {
        private string _content;

        private string _name;

        private Color  _speakerColor;
        private Sprite _sprite;

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