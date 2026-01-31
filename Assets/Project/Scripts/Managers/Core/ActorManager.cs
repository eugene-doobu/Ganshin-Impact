#nullable enable

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GanShin.GanObject
{
    [UsedImplicitly]
    public class ActorManager : ManagerBase
    {
        private readonly Dictionary<long, CreatureObject> _creatureObjects = new();
        private readonly Dictionary<long, PassiveObject>  _passiveObjects  = new();
        private readonly Dictionary<long, SkillObject>    _skillObjects    = new();

        private readonly List<Actor> _pendingAdd    = new();
        private readonly List<Actor> _pendingRemove = new();
        private bool _isIterating;

        private long _currentId;

        [UsedImplicitly]
        public ActorManager()
        {
        }
        
        private Action<Actor?>? _onRegister;
        private Action<Actor?>? _onUnregister;


#region Event & Properties
        public event Action<Actor?>? OnRegister
        {
            add => _onRegister += value;
            remove => _onRegister -= value;
        }
        
        public event Action<Actor?>? OnUnregister
        {
            add => _onUnregister += value;
            remove => _onUnregister -= value;
        }
        
        public IReadOnlyDictionary<long, CreatureObject> CreatureObjects => _creatureObjects;
        public IReadOnlyDictionary<long, PassiveObject>  PassiveObjects  => _passiveObjects;
        public IReadOnlyDictionary<long, SkillObject>    SkillObjects    => _skillObjects;
#endregion Event & Properties

#region Manager
        public override void Initialize()
        {
            _pendingAdd.Clear();
            _pendingRemove.Clear();
            _isIterating = false;
        }

        public override void Tick()
        {
            ProcessPending();

            _isIterating = true;
            PreTickCollection(_creatureObjects);
            PreTickCollection(_passiveObjects);
            PreTickCollection(_skillObjects);

            TickCollection(_creatureObjects);
            TickCollection(_passiveObjects);
            TickCollection(_skillObjects);
            _isIterating = false;
        }

        public override void LateTick()
        {
            _isIterating = true;
            LateTickCollection(_creatureObjects);
            LateTickCollection(_passiveObjects);
            LateTickCollection(_skillObjects);
            _isIterating = false;

            ProcessPending();
        }

        private void ProcessPending()
        {
            // Process pending removals first
            foreach (var actor in _pendingRemove)
            {
                RemoveActorInternal(actor);
            }
            _pendingRemove.Clear();

            // Process pending additions
            foreach (var actor in _pendingAdd)
            {
                RegisterActorInternal(actor);
            }
            _pendingAdd.Clear();
        }

        private void PreTickCollection<T>(Dictionary<long, T> collection) where T : Actor
        {
            foreach (var actor in collection.Values)
            {
                if (actor == null || !actor.isActiveAndEnabled)
                    continue;

                actor.PreTick();
            }
        }

        private void TickCollection<T>(Dictionary<long, T> collection) where T : Actor
        {
            foreach (var actor in collection.Values)
            {
                if (actor == null || !actor.isActiveAndEnabled)
                    continue;

                actor.Tick();
            }
        }

        private void LateTickCollection<T>(Dictionary<long, T> collection) where T : Actor
        {
            foreach (var actor in collection.Values)
            {
                if (actor == null || !actor.isActiveAndEnabled)
                    continue;

                actor.LateTick();
            }
        }

        public void Clear()
        {
            RemoveAllActors();
            _pendingAdd.Clear();
            _pendingRemove.Clear();
            _currentId = 0;
        }
#endregion Manager

#region Actor
        public void RegisterActor(Actor actor)
        {
            if (_isIterating)
            {
                _pendingRemove.Remove(actor);
                if (!_pendingAdd.Contains(actor))
                    _pendingAdd.Add(actor);
                return;
            }

            RegisterActorInternal(actor);
        }

        private void RegisterActorInternal(Actor actor)
        {
            actor.Id = _currentId;
            switch (actor)
            {
                case CreatureObject creatureObject:
                    _creatureObjects[_currentId] = creatureObject;
                    break;
                case PassiveObject passiveObject:
                    _passiveObjects[_currentId] = passiveObject;
                    break;
                case SkillObject skillObject:
                    _skillObjects[_currentId] = skillObject;
                    break;
            }
            _currentId++;
            actor.OnRegister();
            _onRegister?.Invoke(actor);
        }

        public Actor? GetActor(long id)
        {
            if (_creatureObjects.TryGetValue(id, value: out var creatureObject))
                return creatureObject;
            if (_passiveObjects.TryGetValue(id, value: out var passiveObject))
                return passiveObject;
            if (_skillObjects.TryGetValue(id, out var skillObject))
                return skillObject;

            GanDebugger.ActorLogWarning($"Actor with id {id} not found");
            return null;
        }

        public CreatureObject? GetCreatureObject(long id)
        {
            if (_creatureObjects.TryGetValue(id, out var creatureObject))
                return creatureObject;

            GanDebugger.ActorLogWarning($"CreatureObject with id {id} not found");
            return null;
        }

        public PassiveObject? GetPassiveObject(long id)
        {
            if (_passiveObjects.TryGetValue(id, out var passiveObject))
                return passiveObject;

            GanDebugger.ActorLogWarning($"PassiveObject with id {id} not found");
            return null;
        }

        public SkillObject? GetSkillObject(long id)
        {
            if (_skillObjects.TryGetValue(id, out var skillObject))
                return skillObject;

            GanDebugger.ActorLogWarning($"SkillObject with id {id} not found");
            return null;
        }

        private void RemoveActorById(long id)
        {
            if (_creatureObjects.ContainsKey(id))
                _creatureObjects.Remove(id);
            if (_passiveObjects.ContainsKey(id))
                _passiveObjects.Remove(id);
            if (_skillObjects.ContainsKey(id))
                _skillObjects.Remove(id);
        }

        public void RemoveActor(Actor actor)
        {
            if (_isIterating)
            {
                _pendingAdd.Remove(actor);
                if (!_pendingRemove.Contains(actor))
                    _pendingRemove.Add(actor);
                return;
            }

            RemoveActorInternal(actor);
        }

        private void RemoveActorInternal(Actor actor)
        {
            actor.OnUnregister();
            _onUnregister?.Invoke(actor);
            RemoveActorById(actor.Id);
        }

        public void RemoveAllActors()
        {
            _creatureObjects.Clear();
            _passiveObjects.Clear();
            _skillObjects.Clear();
        }
#endregion Actor
    }
}