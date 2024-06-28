using System.Collections.Generic;
using System.Linq;
using com.lonely.common.System.State;

namespace com.lonely.common.RpgSystem.State
{
  public class Tenancies : StateEntity
  {
    public Tenancies() : this(new Dictionary<string, HashSet<string>>(), new Dictionary<string, string>())
    {
    }
    
    public Tenancies(IDictionary<string, HashSet<string>> residentsByLocation, IDictionary<string, string> locationByResident)
    {
      ResidentsByLocation = residentsByLocation;
      LocationByResident = locationByResident;
    }

    public IDictionary<string, HashSet<string>> ResidentsByLocation { get; }
    
    public IDictionary<string, string> LocationByResident { get; }

    public override string Key => $"Tenancies";

    public void SetResident(string resident, string location)
    {
      if (LocationByResident.TryGetValue(resident, out var currentLocation))
      {
        if (currentLocation == location)
        {
          return;
        }
        
        ResidentsByLocation[currentLocation].Remove(resident);
      }

      if (!ResidentsByLocation.ContainsKey(location))
      {
        ResidentsByLocation[location] = new HashSet<string>();
      }
      
      LocationByResident[resident] = location;
      ResidentsByLocation[location].Add(resident);
    }

    public void RemoveResident(string resident)
    {
      if (!LocationByResident.TryGetValue(resident, out var currentLocation)) return;
      
      ResidentsByLocation[currentLocation].Remove(resident);
      LocationByResident.Remove(resident);
    }
    
    public override StateEntity DeepCopy()
    {
      return new Tenancies(ResidentsByLocation.ToDictionary(x => x.Key, x => x.Value.ToHashSet()), LocationByResident.ToDictionary(x => x.Key, x => x.Value));
    }

    public override IEnumerable<IStateEntity> GetChildren()
    {
      return Enumerable.Empty<IStateEntity>();
    }
  }
}