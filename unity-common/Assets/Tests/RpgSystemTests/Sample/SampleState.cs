using System;
using System.Collections.Generic;
using com.lonely.common.EcsSystem;
using com.lonely.common.System.State;
using Tests.RpgSystemTests.Sample.Components;
using Tests.RpgSystemTests.Sample.Entities;

namespace Tests.RpgSystemTests.Sample
{
  internal class SampleState : EcsRoot<SampleState>
  {
    public SampleState(int step) : base(step, null,null, null, null, null)
    {
      Init();

      Tiles = new Dictionary<int, Tile>();
      foreach (var tile in Entities.Get<Tile>())
      {
        Tiles.Add(tile.Location.X, tile);
      }
    }

    public SampleState(int step, TypedListDictionary<Component> components, TypedListDictionary<Entity> entities, TypedListDictionary<Cmd> cmds, TypedListDictionary<Evt> evts, ObserverRegistry<SampleState> observers)
      : base(step, components ?? new TypedListDictionary<Component>(), entities ?? new TypedListDictionary<Entity>(), cmds ?? new TypedListDictionary<Cmd>(), evts ?? new TypedListDictionary<Evt>(), observers)
    {
      Tiles = new Dictionary<int, Tile>();
      foreach (var tile in Entities.Get<Tile>())
      {
        Tiles.Add(tile.Location.X, tile);
      }
    }

    public override string Key => "Root";

    public override StateEntity DeepCopy()
    {
      return new SampleState(Step, Components.DeepCopy(x => x with { }), Entities.DeepCopy(x => x.DeepCopyAs<Entity>()), Cmds.DeepCopy(x => x), Evts.DeepCopy(x => x), Observers);
    }

    public IEnumerable<Character> Characters => Entities.Get<Character>();
    public IDictionary<int, Tile> Tiles { get; init; }

    public void Init()
    {
      Entities.Add(new Tile(new Location() { X = 1 }));
      Entities.Add(new Tile(new Location() { X = 2 }));
      Entities.Add(new Tile(new Location() { X = 3 }));
      Entities.Add(new Tile(new Location() { X = 4 }));

      Entities.Add(new Character(new Id(Guid.NewGuid()), new Location() { X = 1 }));
      Entities.Add(new Character(new Id(Guid.NewGuid()), new Location() { X = 4 }));
    }
  }
}