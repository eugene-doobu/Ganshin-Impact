using System.Collections.Generic;
using GanShin.Dialogue.Base;
using GanShin.Space.Content;
using UnityEngine;
using Zenject;

namespace GanShin
{
    [CreateAssetMenu(menuName = "Installers/SpaceSceneInstaller")]
    public class SpaceSceneInstaller : ScriptableObjectInstaller<SpaceSceneInstaller>
    {
        public const string DialogueImageInfoId = "SpaceSceneInstaller.DialogueImageInfoId";
        
        private const string UIPrefabName            = "Prefabs/UI/Root/Canvas_SpaceScene";
        private const string MinimapCameraPrefabName = "Prefabs/Camera/MinimapCamera";

        [SerializeField] private DialogueImageInfo[] dialogueImageInfos;
        
        private readonly Dictionary<ENpcDialogueImage, Sprite> _dialogueImageInfoDic = new();

        public override void InstallBindings()
        {
            InitializeDialogueImageDict();
            
            Container.BindInstance(_dialogueImageInfoDic).WithId(DialogueImageInfoId);
            
            Container.Bind<Camera>()
                .WithId(MinimapManager.MinimapCameraId)
                .FromComponentInNewPrefabResource(MinimapCameraPrefabName)
                .AsSingle()
                .NonLazy();

            Container.Bind<Canvas>()
                .FromComponentInNewPrefabResource(UIPrefabName)
                .AsSingle()
                .NonLazy();

            Container.Bind(
                    typeof(MinimapManager),
                    typeof(IInitializable),
                    typeof(ILateTickable)
                )
                .To<MinimapManager>()
                .AsSingle()
                .NonLazy();

            Container.Bind(
                    typeof(InventoryManager),
                    typeof(IInitializable),
                    typeof(ITickable)
                )
                .To<InventoryManager>()
                .AsSingle()
                .NonLazy();

            Container.Bind(
                    typeof(DialogueManager),
                    typeof(IInitializable),
                    typeof(ITickable)
                )
                .To<DialogueManager>()
                .AsSingle()
                .NonLazy();
        }

        private void InitializeDialogueImageDict()
        {
            _dialogueImageInfoDic.Clear();
            foreach (var info in dialogueImageInfos)
                _dialogueImageInfoDic.Add(info.type, info.sprite);
        }
    }
}
