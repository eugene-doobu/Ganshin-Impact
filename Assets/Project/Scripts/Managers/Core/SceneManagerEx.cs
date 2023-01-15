using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GanShin.SceneManagement
{
    [UsedImplicitly]
    public class SceneManagerEx
    {
        public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

        public SceneManagerEx()
        {
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }
        
        public void LoadScene(Define.eScene type)
        {
            SceneManager.LoadScene(GetSceneName(type));
        }

        string GetSceneName(Define.eScene type)
        {
            string name = System.Enum.GetName(typeof(Define.eScene), type);
            return name;
        }

        private void OnSceneUnLoaded(Scene scene)
        {
            CurrentScene.Clear();
        }
    }
}

