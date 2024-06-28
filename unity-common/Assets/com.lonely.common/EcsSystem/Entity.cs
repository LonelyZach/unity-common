using System;
using System.Collections.Generic;
using System.Linq;
using com.lonely.common.System.State;

namespace com.lonely.common.EcsSystem
{
  public abstract class Entity : StateEntity
  {
    public Entity(TypedListDictionary<Component> components)
    {
      Components = components ?? new TypedListDictionary<Component>();
    }

    public readonly TypedListDictionary<Component> Components;

    public override IEnumerable<IStateEntity> GetChildren()
    {
      return Enumerable.Empty<IStateEntity>();
    }
    
    public TypedListDictionary<Component> DeepCopyComponents()
    {
      return Components.DeepCopy(x => x with { });
    }

    protected void EnsureComponent<TComponent>(Func<TComponent> builder) where TComponent : Component
    {
      if (!Components.Get<TComponent>().Any())
      {
        Components.Add(builder());
      }
    }
  }
}