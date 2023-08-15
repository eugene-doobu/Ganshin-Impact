using Cysharp.Threading.Tasks;
using GanShin.UI;
using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public class SimpleDemoScene : BaseScene
    {
        private enum EDebugInputGroup
        {
            CHARACTER_CHANGE,
            GLOBAL_UI,
        }
        
        [Inject] private PlayerManager _playerManager;

        [Inject] private SceneManagerEx _sceneManager;
        
        [Inject] private UIManager _uiManager;
        
        [SerializeField] private Define.ePlayerAvatar playerAvatar;

        [SerializeField] private Vector3 startPosition;

        [SerializeField] private EDebugInputGroup debugInputGroup = EDebugInputGroup.GLOBAL_UI;
        
        protected override void Init()
        {
            base.Init();
            ESceneType = Define.eScene.SimpleDemo;
        }

        private void Start()
        {
            var player = _playerManager.SetCurrentPlayer(playerAvatar);
            player.transform.position = startPosition;
        }

        private void Update()
        {
            ProcessDebugInput();
        }
        
        private void ProcessDebugInput()
        {
            ProcessCommonDebugInput();
            switch (debugInputGroup)
            {
                case EDebugInputGroup.CHARACTER_CHANGE:
                    ProcessCharacterChangeDebugInput();
                    break;
                case EDebugInputGroup.GLOBAL_UI:
                    ProcessGlobalUIDebugInput();
                    break;
            }
        }
        
        private void ProcessCommonDebugInput()
        {
            if (Input.GetKeyDown("]"))
                _sceneManager.LoadScene(ESceneType).Forget();
        }

        private void ProcessCharacterChangeDebugInput()
        {
            if (Input.GetKeyDown("1"))
                _playerManager.SetCurrentPlayer(Define.ePlayerAvatar.RIKO);
            
            if (Input.GetKeyDown("2"))
                _playerManager.SetCurrentPlayer(Define.ePlayerAvatar.AI);
            
            if (Input.GetKeyDown("3"))
                _playerManager.SetCurrentPlayer(Define.ePlayerAvatar.MUSCLE_CAT);
        }

        private void ProcessGlobalUIDebugInput()
        {
            if (Input.GetKeyDown("1"))
                _uiManager.SetPopupOk("타이틀", "메시지", () => { Debug.Log("OK"); });
            
            if (Input.GetKeyDown("2"))
                _uiManager.SetPopupOkCancel("타이틀", "메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. 메시지인데 짱김. ", () => { Debug.Log("OK"); }, () => { Debug.Log("Cancel"); });
        }

        public override void Clear()
        {
        }
    }
}