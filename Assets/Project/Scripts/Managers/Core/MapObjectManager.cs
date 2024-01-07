using System.Collections.Generic;
using JetBrains.Annotations;

#nullable enable

namespace GanShin.GanObject
{
    [UsedImplicitly]
    public class MapObjectManager : ManagerBase
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
            RemoveAllMapObjects();
            _currentId = 0;
        }

#endregion Manager

#region MapObject

        public void RegisterMapObject(MapObject mapObject)
        {
            mapObject.Id = _currentId;
            _currentId++;

            switch (mapObject)
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

        public MapObject? GetMapObject(long id)
        {
            if (_creatureObjects.ContainsKey(id))
                return _creatureObjects[id];
            if (_passiveObjects.ContainsKey(id))
                return _passiveObjects[id];
            if (_staticObjects.ContainsKey(id))
                return _staticObjects[id];

            GanDebugger.MapObjectLogWarning($"MapObject with id {id} not found");
            return null;
        }

        public CreatureObject? GetCreatureObject(long id)
        {
            if (_creatureObjects.ContainsKey(id))
                return _creatureObjects[id];

            GanDebugger.MapObjectLogWarning($"CreatureObject with id {id} not found");
            return null;
        }

        public PassiveObject? GetPassiveObject(long id)
        {
            if (_passiveObjects.ContainsKey(id))
                return _passiveObjects[id];

            GanDebugger.MapObjectLogWarning($"PassiveObject with id {id} not found");
            return null;
        }

        public StaticObject? GetStaticObject(long id)
        {
            if (_staticObjects.ContainsKey(id))
                return _staticObjects[id];

            GanDebugger.MapObjectLogWarning($"StaticObject with id {id} not found");
            return null;
        }

        public void RemoveMapObject(long id)
        {
            if (_creatureObjects.ContainsKey(id))
                _creatureObjects.Remove(id);
            if (_passiveObjects.ContainsKey(id))
                _passiveObjects.Remove(id);
            if (_staticObjects.ContainsKey(id))
                _staticObjects.Remove(id);
        }

        public void RemoveMapObject(MapObject mapObject)
        {
            RemoveMapObject(mapObject.Id);
        }

        public void RemoveAllMapObjects()
        {
            _creatureObjects.Clear();
            _passiveObjects.Clear();
            _staticObjects.Clear();
        }

#endregion MapObjecct
    }
}