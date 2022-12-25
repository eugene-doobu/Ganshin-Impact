using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slash.Unity.DataBind.Core.Data;

namespace GanShin.UI
{
    public class WorldFunctionContext : Context
    {
        #region Variables

        private readonly Property<int>   _currentHealth = new Property<int>(100);
        private readonly Property<int>   _maxHealth     = new Property<int>(200);
        private readonly Property<Color> _color         = new Property<Color>(Color.green);

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

        #region Events

        public void OnClickQuit(Context context)
        {
            Debug.Log("Quit");
        }

        #endregion Events
    }
}
