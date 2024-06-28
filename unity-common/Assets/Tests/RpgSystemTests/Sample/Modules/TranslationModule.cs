using System;
using com.lonely.common.CommonUtil;
using com.lonely.common.EcsSystem;
using com.lonely.common.System.Simulation;
using Tests.RpgSystemTests.Sample.Entities;

namespace Tests.RpgSystemTests.Sample.Modules
{
  public class TranslationModule
  {
    internal record Translation(int FromX, int ToX) : Component();
    
    internal record StartTranslation(Guid Entity, int FromX, int ToX) : Cmd();
    internal record CancelTranslations(Guid Entity) : Cmd();
    
    internal record TranslationStarted(Guid Entity, int FromX, int ToX) : Evt(); 
    internal record TranslationCompleted(Guid Entity, int FromX, int ToX) : Evt(); 
    
    internal record StartTranslationsStep : SimulationStep<SampleState>
    {
      public override void Run(SampleState state, int step)
      {
        state
          .Select(x => x.Cmds.Get<StartTranslation>(), out _)
          .Select(x => (Cmd: x, Character: state.Entities.GetSingleOrDefault<Character>(c => c.Id.Value == x.Entity)), out _)
          .Where(x => x.Character != null, out _)
          .Where(x => x.Character.Components.GetFirstOrDefault<Translation>() == null, out _)
          .Where(x => x.Character.Location.X == x.Cmd.FromX, out var operations);

        foreach (var (cmd, character) in operations)
        {
          var translation = new Translation(cmd.FromX, cmd.ToX);
          var evt = new TranslationStarted(character.Id.Value, cmd.FromX, cmd.ToX);
          
          character.Components.Add(translation);
          state.Observers.Notify(state, evt);
          state.SimulateAt(state.Step, "Instant translation");
        }
        
        state.Cmds.Remove<StartTranslation>();
      }
    }
    
    internal record CancelTranslationsStep : SimulationStep<SampleState>
    {
      public override void Run(SampleState state, int step)
      {
        state
          .Select(x => x.Cmds.Get<CancelTranslations>(), out _)
          .Select(x => state.Entities.GetSingleOrDefault<Character>(c => c.Id.Value == x.Entity), out _)
          .Where(x => x != null, out var characters);
        
        foreach (var character in characters)
        {
          character.Components.Remove<Translation>();
        }
        
        state.Cmds.Remove<CancelTranslations>();
      }
    }

    internal record CompleteTranslationsStep : SimulationStep<SampleState>
    {
      public override void Run(SampleState state, int step)
      {
        state
          .Select(x => state.Characters, out var characters)
          .Select(x => (Character: x, ActiveTranslation: x.Components.GetLastOrDefault<Translation>()), out _)
          .Where(x => x.ActiveTranslation != null, out _)
          .Where(x => x.Character.Location.X == x.ActiveTranslation.FromX, out var operations);
        
        foreach (var (character, activeTranslation) in operations)
        {
          var evt = new TranslationCompleted(character.Id.Value, activeTranslation.FromX, activeTranslation.ToX);
          character.Location.X = activeTranslation.ToX;
          state.Observers.Notify(state, evt);
        }

        foreach (var character in characters)
        {
          character.Components.Remove<Translation>();
        }
      }
    }
  }
}