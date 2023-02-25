#nullable enable

using System.Collections;
using System.Collections.Generic;
using GanShin.Content.Creature;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GanShin
{
	[UsedImplicitly]
    public class PlayerManager : IInitializable
    {
#region Define
        private const string PlayerPoolName = "@PlayerPool";

        public struct AvatarPath
        {
            private const string Root ="Character/Avatar";
            public static readonly string Riko = $"{Root}/Riko";
        }

        public struct AvatarBindId
        {
            public const string Riko = "PlayerManager.Riko";
        }
#endregion Define
        [Inject(Id = AvatarBindId.Riko)]
        private PlayerController _riko = null!;
        
        private Transform _playerPool = null!;
        
        public PlayerManager()
        {
            SetPlayerPoolRoot();
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
        
        private void SetPlayerPoolRoot()
        {
            var root = GameObject.Find(PlayerPoolName);
            if (root != null) return;
            root = new GameObject {name = PlayerPoolName };
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
