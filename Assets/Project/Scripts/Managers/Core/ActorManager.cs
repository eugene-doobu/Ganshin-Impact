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
        }

        public override void Tick()
        {
            TickCollection(_creatureObjects);
            TickCollection(_passiveObjects);
            TickCollection(_skillObjects);
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

        public void Clear()
        {
            RemoveAllActors();
            _currentId = 0;
        }
#endregion Manager

#region Actor
        public void RegisterActor(Actor actor)
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
                case SkillObject staticObject:
                    _skillObjects[_currentId] = staticObject;
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
            if (_creatureObjects.ContainsKey(id))
                return _creatureObjects[id];

            GanDebugger.ActorLogWarning($"CreatureObject with id {id} not found");
            return null;
        }

        public PassiveObject? GetPassiveObject(long id)
        {
            if (_passiveObjects.ContainsKey(id))
                return _passiveObjects[id];

            GanDebugger.ActorLogWarning($"PassiveObject with id {id} not found");
            return null;
        }

        public SkillObject? GetStaticObject(long id)
        {
            if (_skillObjects.ContainsKey(id))
                return _skillObjects[id];

            GanDebugger.ActorLogWarning($"StaticObject with id {id} not found");
            return null;
        }

        private void RemoveActor(long id)
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
            actor.OnUnregister();
            _onUnregister?.Invoke(actor);
            RemoveActor(actor.Id);
        }

        public void RemoveAllActors()
        {
            _creatureObjects.Clear();
            _passiveObjects.Clear();
            _skillObjects.Clear();
        }
#endregion MapObjecct
    }
}