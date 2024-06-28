using System;
using System.Collections.Generic;
using System.Linq;
using com.lonely.common.System.State;

namespace com.lonely.common.System.Display
{
  public class DisplayEntityTracker<TState, TLocationFinder>
    where TState : RootStateEntity
    where TLocationFinder : LocationFinderBehaviour
  {
    private IDictionary<Type, IList<DisplayEntityBase<TLocationFinder>>> _displayEntityPrefabsByStateEntityType;
    private IDictionary<string, IList<IDisplayEntity<TLocationFinder>>> _displayEntitiesByKey;
    private TLocationFinder _locationFinder;

    public DisplayEntityTracker(IEnumerable<DisplayEntityBase<TLocationFinder>> displayEntityPrefabs, TLocationFinder locationFinder)
    {
      _displayEntityPrefabsByStateEntityType = new Dictionary<Type, IList<DisplayEntityBase<TLocationFinder>>>();
      _displayEntitiesByKey = new Dictionary<string, IList<IDisplayEntity<TLocationFinder>>>();
      _locationFinder = locationFinder;

      foreach (var prefab in displayEntityPrefabs)
      {
        var type = prefab.StateEntityType;
        if(!_displayEntityPrefabsByStateEntityType.ContainsKey(type))
        {
          _displayEntityPrefabsByStateEntityType.Add(type, new List<DisplayEntityBase<TLocationFinder>>());
        }

        _displayEntityPrefabsByStateEntityType[type].Add(prefab);
      }
    }

    public void Render(TState state, SystemBus bus, float simulationStartTime)
    {
      OrphanAll();
      SyncAll(state, bus, simulationStartTime);
      DestroyOrphans();
    }

    private void SyncAll(IRootStateEntity root, SystemBus bus, float simulationStartTime)
    {
      var allStateEntities = GetAllStateEntitiesRecursive(root);
      foreach (var stateEntity in allStateEntities)
      {
        foreach (var displayEntity in stateEntity.GetDisplays())
        {
          Sync(root, stateEntity.Key, displayEntity, bus, simulationStartTime);
        }
      }
    }

    private void Sync(IRootStateEntity root, string key, object stateEntity, SystemBus bus, float simulationStartTime)
    {
      var compositeKey = $"{stateEntity.GetType().FullName}_{key}";
      if (!_displayEntitiesByKey.ContainsKey(compositeKey))
      {
        var displayEntities = new List<IDisplayEntity<TLocationFinder>>();
        var prefabs = DisplayEntityPrefabsForStateEntity(stateEntity.GetType());
        foreach (var prefab in prefabs)
        {
          var displayEntity = prefab.Instantiate(_locationFinder);
          displayEntities.Add(displayEntity);
          displayEntity.InitUnsafe(root, stateEntity, _locationFinder, simulationStartTime, bus);
        }

        _displayEntitiesByKey.Add(compositeKey, displayEntities);
      }

      var entities = _displayEntitiesByKey[compositeKey];
      foreach (var entity in entities)
      {
        entity.SyncUnsafe(root, stateEntity, _locationFinder, simulationStartTime);
        entity.Orphaned = false;
      }
    }

    private void OrphanAll()
    {
      foreach (var display in AllDisplays())
      {
        display.Orphaned = true;
      }
    }

    private void DestroyOrphans()
    {
      var emptyKeys = new List<string>();
      foreach (var keyedDisplays in _displayEntitiesByKey)
      {
        var destroyed = false;
        foreach (var display in keyedDisplays.Value.Where(x => x.Orphaned))
        {
          display.Destroy();
          destroyed = true;
        }

        if (destroyed)
        {
          emptyKeys.Add(keyedDisplays.Key);
        }
      }

      foreach(var key in emptyKeys)
      {
        _displayEntitiesByKey.Remove(key);
      }
    }

    private IList<IDisplayEntity<TLocationFinder>> DisplayEntityPrefabsForStateEntity(Type stateEntityType)
    {
      if(!_displayEntityPrefabsByStateEntityType.TryGetValue(stateEntityType, out var prefabs))
      {
        return new List<IDisplayEntity<TLocationFinder>>();
      }

      return prefabs.Select<DisplayEntityBase<TLocationFinder>, IDisplayEntity<TLocationFinder>>(x => x).ToList();
    }

    private IEnumerable<IDisplayEntity<TLocationFinder>> AllDisplays()
    {
      foreach (var keyedDisplays in _displayEntitiesByKey)
      {
        foreach (var display in keyedDisplays.Value)
        {
          yield return display;
        }
      }
    }

    private IEnumerable<IStateEntity> GetAllStateEntitiesRecursive(IStateEntity stateEntity)
    {
      var entities = new List<IStateEntity> { stateEntity };
      foreach (var entity in stateEntity.GetChildren())
      {
        entities.AddRange(GetAllStateEntitiesRecursive(entity));
      }
      return entities;
    }
  }
}
