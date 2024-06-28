using System.Collections.Generic;
using System.Linq;
using com.lonely.common.EcsSystem;
using com.lonely.common.System.State;
using Tests.RpgSystemTests.Sample.Components;

namespace Tests.RpgSystemTests.Sample.Entities
{
  internal class Character : Entity
  {
    public Character(Id id, Location location) : base(new TypedListDictionary<Component>())
    {
      Components.Add(id);
      Components.Add(location);
    }

    public Character(TypedListDictionary<Component> components) : base(components)
    {
    }

    public Id Id => Components.Get<Id>().Single();
    public Location Location => Components.Get<Location>().Single();

    public override string Key => $"Character_{Id.Value.ToString()}";

    public override StateEntity DeepCopy()
    {
      return new Character(DeepCopyComponents());
    }
  }
}