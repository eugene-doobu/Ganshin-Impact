using System.Collections;
using System.Collections.Generic;
using GanShin.AssetManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GanShin.SceneManagement
{
	public abstract class BaseScene : MonoBehaviour
	{
		[Inject] private ResourceManager _resource;
		
		public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

		void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
			Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
			if (obj == null)
				_resource.Instantiate("UI/EventSystem").name = "@EventSystem";
		}

		public abstract void Clear();
	}

}
