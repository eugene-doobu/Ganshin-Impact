using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.Space.UI
{
    public class MinimapContext : GanContext
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
    }
}