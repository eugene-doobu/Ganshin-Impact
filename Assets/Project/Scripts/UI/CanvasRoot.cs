using System;
using System.Collections.Generic;
using GanShin.Space.UI;
using GanShin.Utils;
using UnityEngine;

namespace GanShin.UI
{
    public class CanvasRoot : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private List<UIRootBase> currentUIRoots = new();
        
        private void Awake()
        {
            var uiRoots = GetComponentsInChildren<UIRootBase>();
            foreach (var uiRoot in uiRoots)
                currentUIRoots.Add(uiRoot);
        }

        public void Update()
        {
            if (Input.GetKeyDown("["))
                ActiveAllUIRoots(false, typeof(UIInventory));
            
            if (Input.GetKeyDown("p"))
                ActiveAllUIRoots(true, typeof(UIInventory));
        }
        
        public void ActiveAllUIRoots(bool value, params Type[] types)
        {
            foreach (var uiRoot in currentUIRoots)
            {
                if (uiRoot.GetType() == typeof(CanvasRoot)) continue;
                
                if (value) uiRoot.Show();
                else uiRoot.Hide();
            }
        }
    }
}
