#nullable enable

using System;
using GanShin.Content.Creature;
using GanShin.UI;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace GanShin
{
    [UsedImplicitly]
    public class PlayerManager : IInitializable
    {
#region Internal Class

        public class PlayerAvatarContext
        {
            public UIHpBarContext? RikoHpBarContext      { get; }
            public UIHpBarContext? AIHpBarContext        { get; }
            public UIHpBarContext? MuscleCatHpBarContext { get; }

            public PlayerAvatarContext()
            {
                RikoHpBarContext      = Activator.CreateInstance(typeof(UIHpBarContext)) as UIHpBarContext;
                AIHpBarContext        = Activator.CreateInstance(typeof(UIHpBarContext)) as UIHpBarContext;
                MuscleCatHpBarContext = Activator.CreateInstance(typeof(UIHpBarContext)) as UIHpBarContext;
            }
        }

#endregion Internal Class

#region Define

        private const string PlayerPoolName = "@PlayerPool";

        public struct AvatarPath
        {
            private const          string Root = "Character/Avatar";
            public static readonly string Riko = $"{Root}/Riko";
        }

        public struct AvatarBindId
        {
            public const string Riko = "PlayerManager.Riko";
        }

#endregion Define

        [Inject(Id = AvatarBindId.Riko)] private PlayerController _riko = null!;

        private PlayerAvatarContext _avatarContext;

        private Transform _playerPool = null!;

        public PlayerManager()
        {
            SetPlayerPoolRoot();
            _avatarContext = new PlayerAvatarContext();
        }

        public PlayerController? GetPlayer(Define.ePlayerAvatar avatar)
        {
            // TODO: 캐릭터 변경 로직으로 변경
            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    return _riko;
                default:
                    return null;
            }
        }

        public UIHpBarContext? GetUIHpBarContext(Define.ePlayerAvatar avatar)
        {
            switch (avatar)
            {
                case Define.ePlayerAvatar.RIKO:
                    return _avatarContext.RikoHpBarContext;
                default:
                    return null;
            }
        }

        private void SetPlayerPoolRoot()
        {
            var root = GameObject.Find(PlayerPoolName);
            if (root != null) return;
            root = new GameObject {name = PlayerPoolName};
            Object.DontDestroyOnLoad(root);

            _playerPool = root.transform;
        }

        public void Initialize()
        {
            _riko.transform.SetParent(_playerPool);
            _riko.gameObject.SetActive(false);
            _riko.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}