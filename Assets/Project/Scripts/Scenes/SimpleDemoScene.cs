using Cysharp.Threading.Tasks;
using GanShin.UI;
using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public class SimpleDemoScene : SpaceScene
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
                _sceneManager.LoadScene(Define.eScene.SIMPLE_DEMO).Forget();
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
            
            if (Input.GetKeyDown("3"))
                _uiManager.SetToast("테스트1", "퀸카 암핫", EToastType.DEFAULT);

            if (Input.GetKeyDown("4"))
                _uiManager.SetToast("테스트2", "그냥 살다 보니 어느새 난 여기 사실 별거 없지 잘은 해 밥벌이", EToastType.NOTIFICATION);
            
            if (Input.GetKeyDown("5"))                
                _uiManager.SetToast("테스트3", "Don't ever say it's over if I'm breathin, Racin' to the moonlight and I'm speedin, I'm headed to the stars, ready to go far, I'm star walkin'", EToastType.WARNING);
            
            if (Input.GetKeyDown("6"))
                _uiManager.SetToast("테스트4", "꺼억", EToastType.ERROR);
            
            if (Input.GetKeyDown("7"))
                _uiManager.ShowLoadingUI(GetHashCode());
            
            if (Input.GetKeyDown("8"))
                _uiManager.HideLoadingUI(GetHashCode());
        }

        public override void Clear()
        {
        }
    }
}