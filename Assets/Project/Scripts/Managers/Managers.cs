using GanShin.InputSystem;
using GanShin.UI;
using GanShin.SceneManagement;
using GanShin.AssetManagement;
using GanShin.Sound;
using GanShin.CameraSystem;
using UnityEngine;

namespace GanShin
{
	public class Managers : MonoBehaviour
    {
        static Managers s_instance; // 유일성이 보장된다
        static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다
    
    	#region Contents
    	#endregion
    
    	#region Core
    	private DataManager          _data          = new();
    	private PoolManager          _pool          = new();
        private ResourceManager      _resource      = new();
        private SceneManagerEx       _scene         = new();
        private SoundManager         _sound         = new();
        private UIManager            _ui            = new();
        private InputSystemManager   _input         = new();
        private VirtualCameraManager _virtualCamera = new();
    
        public static DataManager          Data          => Instance._data;
        public static PoolManager          Pool          => Instance._pool;
        public static ResourceManager      Resource      => Instance._resource;
        public static SceneManagerEx       Scene         => Instance._scene;
        public static SoundManager         Sound         => Instance._sound;
        public static UIManager            UI            => Instance._ui;
        public static InputSystemManager   Input         => Instance._input;
        public static VirtualCameraManager VirtualCamera => Instance._virtualCamera;

        #endregion
    
        // TODO: 씬 오브젝트에 의존하지 않도록 변경 필요
    	void Awake()
        {
    	}
    
        void Update()
        {
    
        }
    
        static void Init()
        {   
            if (s_instance == null)
            {
    			GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                }
    
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();
    
                s_instance._data.Init();
                s_instance._pool.Init();
                s_instance._sound.Init();
                s_instance._input.Init();
            }		
    	}
    
        public static void Clear()
        {
            Sound.Clear();
            Scene.Clear();
            UI.Clear();
            Pool.Clear();
        }
    }
}
