#nullable enable

using System;
using System.Runtime.CompilerServices;
using GanShin.GanObject;
using GanShin.UI;
using JetBrains.Annotations;

namespace GanShin.Space.UI
{
	// TODO: CreatureObjectContext에서 ActorContext로 변경
	[UsedImplicitly]
	public class ActorManagerContext : CollectionManagerContext<long, CreatureObjectContext>
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
				else AddAllActor();
				
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
		
		public Func<Actor, bool>? CustomDisplayFilter { get; set; }
#endregion Fields/Properties

#region Initialize
		public ActorManagerContext()
		{
			// TODO: Context에 Zenject Container 주입
			// TODO: ObjectManager에 있는 오브젝트 목록 긁어와서 Register
		}
#endregion Initialize

#region EventHandler
		private void OnActorAdded(Actor? actor)
		{
			
		}
		
		private void OnActorRemoved(Actor? actor)
		{
			
		}
		
		private void RegisterObserver(Actor? actor)
		{
			if (actor == null) return;
			
			actor.OnBecomeOccluded += OnBecomeOccluded;
			actor.OnBecomeUnoccluded += OnBecomeUnoccluded;
			
			AddActor(actor);
		}
		
		private void DeleteObserver(Actor? actor)
		{
			if (actor == null) return;
			
			actor.OnBecomeOccluded -= OnBecomeOccluded;
			actor.OnBecomeUnoccluded -= OnBecomeUnoccluded;
			
			RemoveActor(actor);
		}
#endregion EventHandler

#region Add/Remove Object
		private void AddActor(Actor actor)
		{
			// TODO: 컬링그룹 조건 추가
			if (!_enable || Contains(actor.Id)) return;
			
			if (!IsMatchingCondition(actor)) return;
			
			// TODO: 구체적인 타입 주입
			// Add(actor.Id, new CreatureObjectContext());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsMatchingCondition(Actor actor)
		{
			return _type switch
			{
				eDisplayType.ALL    => true,
				eDisplayType.OWN    => actor.IsMine,
				eDisplayType.OTHERS => !actor.IsMine,
				eDisplayType.NONE   => false,
				eDisplayType.CUSTOM => CustomDisplayFilter?.Invoke(actor) ?? false,
				_                   => false,
			};
		}

		private void RemoveActor(Actor actor)
		{
			Remove(actor.Id);
		}

		private void AddAllActor()
		{
			// TODO
		}
		
		private void RefreshItemContexts()
		{
			Clear();
			AddAllActor();
		}
#endregion Add/Remove Object
        
#region CullingGroup
		private void OnBecomeUnoccluded(Actor? actor)
		{
		}

		private void OnBecomeOccluded(Actor? actor)
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