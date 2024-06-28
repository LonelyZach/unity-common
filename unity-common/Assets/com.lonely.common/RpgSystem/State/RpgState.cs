using System.Collections.Generic;
using System.Linq;
using com.lonely.common.System.State;

namespace com.lonely.common.RpgSystem.State
{
  public class RpgState : RootStateEntity
  {
    public RpgState(int step) : this(step, new List<Character>(), new List<Hex>(), new Tenancies())
    {
    }
    
    public RpgState(int step, IList<Character> characters, IList<Hex> locations, Tenancies tenancies) : base(step)
    {
      Characters = characters;
      Locations = locations;
      Tenancies = tenancies;
    }

    public override string Key => "State";

    public IList<Character> Characters { get; }
    public IList<Hex> Locations { get; }
    public Tenancies Tenancies { get; }
    
    public override StateEntity DeepCopy()
    {
      return new RpgState(Step, Characters.Select(x => x.DeepCopyAs<Character>()).ToList(), Locations.Select(x => x.DeepCopyAs<Hex>()).ToList(), Tenancies.DeepCopyAs<Tenancies>());
    }

    public override IEnumerable<IStateEntity> GetChildren()
    {
      var children = new List<IStateEntity>();
      children.AddRange(Characters);
      children.AddRange(Locations);
      children.Add(Tenancies);
      return children;
    }
  }
}