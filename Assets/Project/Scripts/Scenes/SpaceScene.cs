using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public abstract class SpaceScene : BaseScene
    {
        private const string UIPrefabName = "Prefabs/UI/Root/Canvas_SpaceScene";

        public static void InstallSpaceUi(DiContainer container)
        {
        }
    }
}
