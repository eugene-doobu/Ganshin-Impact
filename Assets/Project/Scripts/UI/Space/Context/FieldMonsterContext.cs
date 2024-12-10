using JetBrains.Annotations;
using UnityEngine;

namespace GanShin.UI.Space
{
    public class FieldMonsterContext : CreatureObjectContext
    {
        private Vector3 _position;

        [UsedImplicitly]
        public Vector3 Position
        {
            get => _position;
            set
            {
                var rectPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, value);
                _position = rectPosition;
                OnPropertyChanged();
            }
        }
    }
}
