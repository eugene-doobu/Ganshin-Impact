using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GanShin.SceneManagement
{
	public abstract class BaseScene : MonoBehaviour
	{
		public Define.eScene ESceneType { get; protected set; } = Define.eScene.Unknown;

		void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
		}

		public abstract void Clear();
	}

}
