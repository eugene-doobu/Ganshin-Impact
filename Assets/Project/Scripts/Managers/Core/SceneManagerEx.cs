using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GanShin.SceneManagement
{
    public class SceneManagerEx
    {
        public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

        public SceneManagerEx()
        {
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
        }
        
        public void LoadScene(Define.Scene type)
        {
            SceneManager.LoadScene(GetSceneName(type));
        }

        string GetSceneName(Define.Scene type)
        {
            string name = System.Enum.GetName(typeof(Define.Scene), type);
            return name;
        }

        private void OnSceneUnLoaded(Scene scene)
        {
            CurrentScene.Clear();
        }
    }
}

