using Cysharp.Threading.Tasks;
using GanShin.Space.Content;
using GanShin.UI;
using UnityEngine;
using Zenject;

namespace GanShin.SceneManagement
{
    public class SimpleDemoScene : SpaceScene
    {
        private enum EDebugInputGroup
        {
            NONE,
            CHARACTER_CHANGE,
            GLOBAL_UI,
            DIALOGUE,
        }
        
        [Inject] private PlayerManager _playerManager;
        [Inject] private SceneManagerEx _sceneManager;
        [Inject] private UIManager _uiManager;
        [Inject] private DialogueManager _dialogueManager;
        
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
                case EDebugInputGroup.NONE:
                    break;
                case EDebugInputGroup.CHARACTER_CHANGE:
                    ProcessCharacterChangeDebugInput();
                    break;
                case EDebugInputGroup.GLOBAL_UI:
                    ProcessGlobalUIDebugInput();
                    break;
                case EDebugInputGroup.DIALOGUE:
                    ProcessDialogue();
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

        private void ProcessDialogue()
        {
            if (Input.GetKeyDown("1"))
                _dialogueManager.StartDialogue();
            
            if (Input.GetKeyDown("2"))
                _dialogueManager.SetString("hello");
            
            if (Input.GetKeyDown("3"))
                _dialogueManager.SetString("밥을 어디 옷을 천하를 불어 품으며, 소담스러운 쓸쓸하랴? 사라지지 하는 인생에 이것이다. 더운지라 열락의 실현에 안고, 그들은 우리 불러 그와 가슴이 것이다. 위하여서 인생의 물방아 끓는다. 생명을 수 소금이라 거친 몸이 그림자는 타오르고 놀이 봄바람이다. 품으며, 구하기 내는 인생에 없는 때에, 실로 기쁘며, 인간은 사막이다.");
            
            if (Input.GetKeyDown("4"))
                _dialogueManager.SetString("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting,");
        }

        public override void Clear()
        {
        }
    }
}