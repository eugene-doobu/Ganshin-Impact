using System;
using System.Collections.Generic;
using System.Linq;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.UI
{
    public class CanvasRoot : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private List<UIRootBase> currentUIRoots = new();
        
        private void Awake()
        {
            var uiRoots = GetComponentsInChildren<UIRootBase>(true);
            foreach (var uiRoot in uiRoots)
            {
                currentUIRoots.Add(uiRoot);
                uiRoot.InjectCanvasRoot(this);
            }
        }
        
        public void ActiveAllUIRoots(bool value, params Type[] ignoreTypes)
        {
            foreach (var uiRoot in currentUIRoots)
            {
                if (ignoreTypes.Contains(uiRoot.GetType())) continue;
                
                if (value) uiRoot.Show();
                else uiRoot.Hide();
            }
        }
    }
}
