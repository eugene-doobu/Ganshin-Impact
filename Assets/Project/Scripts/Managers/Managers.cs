using System;
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
        static Managers s_instance;
        static Managers Instance { get { Init(); return s_instance; } }

        public static bool InstanceExist { get; private set; } = false;
    
    	#region Contents
    	#endregion
    
    	#region Core
        
    	private DataManager        _data     = new();
    	private PoolManager        _pool     = new();
        private ResourceManager    _resource = new();
        private SceneManagerEx     _scene    = new();
        private SoundManager       _sound    = new();
        private UIManager          _ui       = new();
        private InputSystemManager _input    = new();
        private CameraManager      _camera   = new();
    
        public static DataManager        Data     => Instance._data;
        public static PoolManager        Pool     => Instance._pool;
        public static ResourceManager    Resource => Instance._resource;
        public static SceneManagerEx     Scene    => Instance._scene;
        public static SoundManager       Sound    => Instance._sound;
        public static UIManager          UI       => Instance._ui;
        public static InputSystemManager Input    => Instance._input;
        public static CameraManager      Camera   => Instance._camera;

        #endregion

        #region UnityEvent
        
        private void OnInit()
        {
	        s_instance._data.Init();
	        s_instance._pool.Init();
	        s_instance._sound.Init();
	        s_instance._input.Init();
	        s_instance._camera.Init();
        } 
        
        private void Awake()
        {
        }

        private void Start()
        {
	        
        }
    
        private void Update()
        {
	        s_instance._camera?.OnUpdate();
        }
        
        private void LateUpdate()
        {
	        s_instance._camera?.OnLateUpdate();
        }
    
        public static void Clear()
        {
	        Sound.Clear();
	        Scene.Clear();
	        UI.Clear();
	        Pool.Clear();
        }

        private void OnDestroy()
        {
	        InstanceExist = false;
        }
        
        #endregion UnityEvent 
        
        public static void Init()
        {
	        if (s_instance != null) return;
	        
	        GameObject go = GameObject.Find("@Managers");
	        if (go == null)
	        {
		        go = new GameObject { name = "@Managers" };
		        go.AddComponent<Managers>();
	        }
    
	        DontDestroyOnLoad(go);
	        s_instance = go.GetComponent<Managers>();
	        OnInit();
	        
	        InstanceExist = true;
        }
    }
}
