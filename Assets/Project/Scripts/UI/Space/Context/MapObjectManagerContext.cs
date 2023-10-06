#nullable enable

using System;
using System.Runtime.CompilerServices;
using GanShin.GanObject;
using GanShin.UI;
using JetBrains.Annotations;

namespace GanShin.Space.UI
{
	// TODO: CreatureObjectContext에서 MapObjectContext로 변경
	[UsedImplicitly]
	public class MapObjectManagerContext : CollectionManagerContext<long, CreatureObjectContext>
	{
		public enum eDisplayType
		{
			ALL,
			OWN,
			OTHERS,
			NONE,
			CUSTOM,
		}

#region Fields/Properties
		private bool _enable;
		
		public bool Enable
		{
			get => _enable;
			set
			{
				if (_enable == value) return;
				
				_enable = value;

				if (!value) Clear();
				else AddAllObjects();
				
				OnPropertyChanged();
			}
		}
		
		private eDisplayType _type = eDisplayType.ALL;
		
		public eDisplayType Type
		{
			get => _type;
			set
			{
				if (_type == value) return;
				
				if (value != eDisplayType.CUSTOM)
					CustomDisplayFilter = null;
				
				_type = value;
				OnPropertyChanged();
				RefreshItemContexts();
			}
		}
		
		public Func<MapObject, bool>? CustomDisplayFilter { get; set; }
#endregion Fields/Properties

#region Initialize
		public MapObjectManagerContext()
		{
			// TODO: Context에 Zenject Container 주입
			// TODO: ObjectManager에 있는 오브젝트 목록 긁어와서 Register
		}
#endregion Initialize

#region EventHandler
		private void OnMapObjectAdded(MapObject? mapObject)
		{
			
		}
		
		private void OnMapObjectRemoved(MapObject? mapObject)
		{
			
		}
		
		private void RegisterObserver(MapObject? mapObject)
		{
			if (mapObject == null) return;
			
			mapObject.OnBecomeOccluded += OnBecomeOccluded;
			mapObject.OnBecomeUnoccluded += OnBecomeUnoccluded;
			
			AddMapObject(mapObject);
		}
		
		private void DeleteObserver(MapObject? mapObject)
		{
			if (mapObject == null) return;
			
			mapObject.OnBecomeOccluded -= OnBecomeOccluded;
			mapObject.OnBecomeUnoccluded -= OnBecomeUnoccluded;
			
			RemoveMapObject(mapObject);
		}
#endregion EventHandler

#region Add/Remove Object
		private void AddMapObject(MapObject mapObject)
		{
			// TODO: 컬링그룹 조건 추가
			if (!_enable || Contains(mapObject.Id)) return;
			
			if (!IsMatchingCondition(mapObject)) return;
			
			// TODO: 구체적인 타입 주입
			// Add(mapObject.Id, new CreatureObjectContext());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsMatchingCondition(MapObject mapObject)
		{
			return _type switch
			{
				eDisplayType.ALL    => true,
				eDisplayType.OWN    => mapObject.IsMine,
				eDisplayType.OTHERS => !mapObject.IsMine,
				eDisplayType.NONE   => false,
				eDisplayType.CUSTOM => CustomDisplayFilter?.Invoke(mapObject) ?? false,
				_                   => false,
			};
		}

		private void RemoveMapObject(MapObject mapObject)
		{
			Remove(mapObject.Id);
		}

		private void AddAllMapObject()
		{
			// TODO
		}
		
		private void RefreshItemContexts()
		{
			Clear();
			AddAllMapObject();
		}
#endregion Add/Remove Object
        
#region CullingGroup
		private void OnBecomeUnoccluded(MapObject? mapObject)
		{
		}

		private void OnBecomeOccluded(MapObject? mapObject)
		{
		}
#endregion CullingGroup

		protected override void OnDispose()
		{
			// TODO: 등록된 이벤트 제거
			base.OnDispose();
		}
	}
}