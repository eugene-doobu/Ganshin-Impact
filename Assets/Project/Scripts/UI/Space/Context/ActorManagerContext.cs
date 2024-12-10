#nullable enable

using System;
using System.Runtime.CompilerServices;
using GanShin.GanObject;
using GanShin.UI;
using JetBrains.Annotations;

namespace GanShin.Space.UI
{
	[UsedImplicitly]
	public abstract class ActorManagerContext : CollectionManagerContext<long, CreatureObjectContext>
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
		protected ActorManagerContext()
		{
			var actorManager = ProjectManager.Instance.GetManager<ActorManager>();

			if (actorManager == null)
			{
				GanDebugger.ActorLogError("Failed to get actor manager");
				return;
			}

			AddAllActor();

			actorManager.OnRegister += OnActorAdded;
			actorManager.OnUnregister += OnActorRemoved;
		}

        protected override void OnDispose()
        {
	        base.OnDispose();

	        var actorManager = ProjectManager.Instance.GetManager<ActorManager>();
	        if (actorManager == null) return;

	        actorManager.OnRegister -= OnActorAdded;
	        actorManager.OnUnregister -= OnActorRemoved;
        }
#endregion Initialize

#region EventHandler
		private void OnActorAdded(Actor? actor)
		{
			RegisterObserver(actor);
		}

		private void OnActorRemoved(Actor? actor)
		{
			DeleteObserver(actor);
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

			RefreshOcclusionState(actor);
			AddContext(actor);
		}

		protected abstract void AddContext(Actor actor);

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
			OnBecomeOccluded(actor);
			Remove(actor.Id);
		}

		private void AddAllActor()
		{
			var actorManager = ProjectManager.Instance.GetManager<ActorManager>();
			if (actorManager == null) return;

			var creatureObjects = actorManager.CreatureObjects;
			foreach (var creatureObject in creatureObjects.Values)
			{
				RegisterObserver(creatureObject);
				RefreshOcclusionState(creatureObject);
			}
		}

		private void RefreshItemContexts()
		{
			Clear();
			AddAllActor();
		}
#endregion Add/Remove Object

#region CullingGroup
		private void RefreshOcclusionState(Actor actor)
		{
			if (actor.IsOccluded)
				OnBecomeOccluded(actor);
			else
				OnBecomeUnoccluded(actor);
		}

		private void OnBecomeUnoccluded(Actor? actor)
		{
			if (actor == null) return;
			var uiManager = ProjectManager.Instance.GetManager<UIManager>();
			uiManager?.AddNearByObject(actor);

			var hasContext = TryGet(actor.Id, out var context);
			if (!hasContext || context == null) return;

			context.IsEnable = true;
		}

		private void OnBecomeOccluded(Actor? actor)
		{
			if (actor == null) return;
			var uiManager = ProjectManager.Instance.GetManager<UIManager>();
			uiManager?.RemoveNearByObject(actor);

			var hasContext = TryGet(actor.Id, out var context);
			if (!hasContext || context == null) return;

			context.IsEnable = false;
		}
#endregion CullingGroup
    }
}