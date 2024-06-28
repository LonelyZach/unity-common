using System.Collections.Generic;
using System.Linq;
using com.lonely.common.System.State;

namespace com.lonely.common.RpgSystem.State
{
  public class Character : StateEntity
  {
    public Character(string id)
    {
      Id = id;
    }

    public string Id { get; }
    
    public override string Key => $"Character_{Id}";
    
    public override StateEntity DeepCopy()
    {
      return new Character(Id);
    }

    public override IEnumerable<IStateEntity> GetChildren()
    {
      return Enumerable.Empty<IStateEntity>();
    }
  }
}