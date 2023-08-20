using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GanShin.SceneManagement
{
    // TODO: Zenject Installer를 통한 BaseScene 객체 생성?
    public abstract class BaseScene : MonoBehaviour
    {
        void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
        }

        public abstract void Clear();
    }
}