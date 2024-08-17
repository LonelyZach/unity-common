using System.Linq;
using NUnit.Framework;
using Tests.RpgSystemTests.Sample;
using Tests.RpgSystemTests.Sample.Modules;
using Tests.RpgSystemTests.Sample.Statuses;
using Stack = Tests.RpgSystemTests.Sample.Components.Stack;

namespace Tests.RpgSystemTests
{
  [TestFixture]
  public class SampleStateTests
  {
    private com.lonely.common.System.System<SampleState> SetupSystem()
    {
      var state = new SampleState(0);
      var system = new com.lonely.common.System.System<SampleState>(state, null);
      
      system.Simulation.Register(new TranslationModule.StartTranslationsStep());
      system.Simulation.Register(new TranslationModule.CancelTranslationsStep());
      system.Simulation.Register(new TranslationModule.CompleteTranslationsStep());
      
      return system;
    }

    [Test]
    public void TestSimpleTranslation()
    {
      var system = SetupSystem();
      var state = system.GetState();

      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 4));
      
      state.Cmds.Add(new TranslationModule.StartTranslation(state.Characters.First(x => x.Location.X == 1).Id.Value, 1, 2));
      state.SimulateNow("Init");
      
      system.Simulation.Step(0);

      Assert.IsNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 2));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 4));
    }

    [Test]
    public void TestSimpleTranslationIgnoredByCopiedState()
    {
      var system = SetupSystem();
      var state = system.GetState();

      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 4));

      state.Cmds.Add(new TranslationModule.StartTranslation(state.Characters.First(x => x.Location.X == 1).Id.Value, 1, 2));
      state.SimulateNow("Init");

      var copiedSimulation = system.DeepCopy();
      
      state.Cmds.Add(new TranslationModule.StartTranslation(state.Characters.First(x => x.Location.X == 4).Id.Value, 4, 3));

      system.Simulation.Step(0);
      copiedSimulation.Simulation.Step(0);

      Assert.IsNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 2));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 3));
      Assert.IsNull(state.Characters.FirstOrDefault(x => x.Location.X == 4));

      Assert.IsNull(copiedSimulation.GetState().Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNotNull(copiedSimulation.GetState().Characters.FirstOrDefault(x => x.Location.X == 2));
      Assert.IsNull(copiedSimulation.GetState().Characters.FirstOrDefault(x => x.Location.X == 3));
      Assert.IsNotNull(copiedSimulation.GetState().Characters.FirstOrDefault(x => x.Location.X == 4));
    }
    
    [Test]
    public void TestRootedStatusStopsWalk()
    {
      var system = SetupSystem();
      var state = system.GetState();

      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 4));

      state.Characters.First(x => x.Location.X == 1).Components.Add(new Stack(new Rooted()) { Magnitude = 1 });
      
      state.Cmds.Add(new TranslationModule.StartTranslation(state.Characters.First(x => x.Location.X == 1).Id.Value, 1, 2));
      state.SimulateNow("Init");

      system.Simulation.Step(0);
      
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNull(state.Characters.FirstOrDefault(x => x.Location.X == 2));
    }
    
    [Test]
    public void TestLockedStatusAllowsWalk()
    {
      var system = SetupSystem();
      var state = system.GetState();

      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 4));

      state.Characters.First(x => x.Location.X == 1).Components.Add(new Stack(new Locked()) { Magnitude = 1 });
      
      state.Cmds.Add(new TranslationModule.StartTranslation(state.Characters.First(x => x.Location.X == 1).Id.Value, 1, 2, false));
      state.SimulateNow("Init");

      system.Simulation.Step(0);
      
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 2));
      Assert.IsNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
    }
    
    [Test]
    public void TestLockedStatusStopsTeleport()
    {
      var system = SetupSystem();
      var state = system.GetState();

      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 4));

      state.Characters.First(x => x.Location.X == 1).Components.Add(new Stack(new Locked()) { Magnitude = 1 });
      
      state.Cmds.Add(new TranslationModule.StartTranslation(state.Characters.First(x => x.Location.X == 1).Id.Value, 1, 2, true));
      state.SimulateNow("Init");

      system.Simulation.Step(0);
      
      Assert.IsNotNull(state.Characters.FirstOrDefault(x => x.Location.X == 1));
      Assert.IsNull(state.Characters.FirstOrDefault(x => x.Location.X == 2));
    }
  }
}