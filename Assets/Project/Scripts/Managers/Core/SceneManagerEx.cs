using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GanShin.SceneManagement
{
    public class SceneManagerEx
    {
        public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

        public void LoadScene(Define.eScene type)
        {
            Managers.Clear();

            SceneManager.LoadScene(GetSceneName(type));
        }

        string GetSceneName(Define.eScene type)
        {
            string name = System.Enum.GetName(typeof(Define.eScene), type);
            return name;
        }

        public void Clear()
        {
            CurrentScene.Clear();
        }
    }
}

