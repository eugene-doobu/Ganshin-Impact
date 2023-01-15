using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GanShin
{
	public static class Extension
	{
		public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
		{
			return Util.GetOrAddComponent<T>(go);
		}
		
		public static Transform RecursiveFind(this Transform parent, string name)
		{
			foreach (Transform child in parent)
			{
				if(child.name == name)
				{
					return child;
				}
				var found = RecursiveFind(child, name);
				if (found != null) 
					return found;
			}
			return null;
		}

		public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.eUIEvent type = Define.eUIEvent.Click)
		{
			UI_Base.BindEvent(go, action, type);
		}

		public static bool IsValid(this GameObject go)
		{
			return go != null && go.activeSelf;
		}
	}
}
