using Slash.Unity.DataBind.Core.Data;
using UnityEngine;

namespace GanShin.UI
{
    public class WorldFunctionContext : Context
    {
#region Events

        public void OnClickQuit(Context context)
        {
            Debug.Log("Quit");
        }

#endregion Events

#region Variables

        private readonly Property<int>   _currentHealth = new(100);
        private readonly Property<int>   _maxHealth     = new(200);
        private readonly Property<Color> _color         = new(Color.green);

#endregion Variables

#region Properties

        public int CurrentHealth
        {
            get => _currentHealth.Value;
            set => _currentHealth.Value = value;
        }

        public int MaxHealth
        {
            get => _maxHealth.Value;
            set => _maxHealth.Value = value;
        }

        public Color Color
        {
            get => _color.Value;
            set => _color.Value = value;
        }

#endregion Properties
    }
}