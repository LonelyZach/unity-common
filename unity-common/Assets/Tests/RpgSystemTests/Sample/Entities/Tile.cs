using System.Linq;
using com.lonely.common.EcsSystem;
using com.lonely.common.System.State;
using Tests.RpgSystemTests.Sample.Components;

namespace Tests.RpgSystemTests.Sample.Entities
{
  internal class Tile : Entity
  {
    public Tile(Location location) : base(new TypedListDictionary<Component>())
    {
      Components.Add(location);
    }

    public Tile(TypedListDictionary<Component> components) : base(components)
    {
    }

    public Location Location => Components.Get<Location>().Single();

    public override string Key => $"Tile_{Location.X}";
    
    public override StateEntity DeepCopy()
    {
      return new Tile(DeepCopyComponents());
    }
  }
}