using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GanShin
{
    public class Initializer : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void AppInitialize()
        {
            Managers.Init();
        }
    }
}
