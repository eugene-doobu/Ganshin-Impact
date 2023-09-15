using System;
using GanShin.UI;
using JetBrains.Annotations;

namespace GanShin.Space.UI
{
    public class InteractionItemContext : GanContext
    {
        private string _nameProperty;
        
        public event Action OnClickEvent;

        [UsedImplicitly]
        public string Name
        {
            get => _nameProperty;
            set
            {
                _nameProperty = value;
                OnPropertyChanged();
            }
        }

        [UsedImplicitly]
        public void OnClick()
        {
            OnClickEvent?.Invoke();
        }
    }
}