using System.Collections.Generic;
using JetBrains.Annotations;

#nullable enable

namespace GanShin.GanObject
{
    [UsedImplicitly]
    public class ActorManager
    {
        private Dictionary<long, CreatureObject> _creatureObjects = new();
        private Dictionary<long, PassiveObject>  _passiveObjects  = new();
        private Dictionary<long, StaticObject>   _staticObjects   = new();

        private long _currentId = 0;

#region Manager

        public void Init()
        {
        }

        public void OnUpdate()
        {
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
            _currentId++;

            switch (actor)
            {
                case CreatureObject creatureObject:
                    _creatureObjects[_currentId] = creatureObject;
                    break;
                case PassiveObject passiveObject:
                    _passiveObjects[_currentId] = passiveObject;
                    break;
                case StaticObject staticObject:
                    _staticObjects[_currentId] = staticObject;
                    break;
            }
        }

        public Actor? GetActor(long id)
        {
            if (_creatureObjects.ContainsKey(id))
                return _creatureObjects[id];
            if (_passiveObjects.ContainsKey(id))
                return _passiveObjects[id];
            if (_staticObjects.ContainsKey(id))
                return _staticObjects[id];

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

        public StaticObject? GetStaticObject(long id)
        {
            if (_staticObjects.ContainsKey(id))
                return _staticObjects[id];

            GanDebugger.ActorLogWarning($"StaticObject with id {id} not found");
            return null;
        }

        public void RemoveActor(long id)
        {
            if (_creatureObjects.ContainsKey(id))
                _creatureObjects.Remove(id);
            if (_passiveObjects.ContainsKey(id))
                _passiveObjects.Remove(id);
            if (_staticObjects.ContainsKey(id))
                _staticObjects.Remove(id);
        }

        public void RemoveActor(Actor actor)
        {
            RemoveActor(actor.Id);
        }

        public void RemoveAllActors()
        {
            _creatureObjects.Clear();
            _passiveObjects.Clear();
            _staticObjects.Clear();
        }

#endregion MapObjecct
    }
}