using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin.Director
{
    public class DemoSceneDirector : MonoBehaviour
    {
        [SerializeField] private Light _light;

        private void Awake()
        {
            Debug.LogError("tset");
        }
    }
}
