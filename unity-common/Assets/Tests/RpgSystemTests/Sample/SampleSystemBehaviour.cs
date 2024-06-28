using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using com.lonely.common.EcsSystem;
using com.lonely.common.System.Simulation;
using com.lonely.common.System.State;
using Tests.RpgSystemTests.Sample.Components;
using Tests.RpgSystemTests.Sample.Entities;
using Tests.RpgSystemTests.Sample.Modules;

namespace Tests.RpgSystemTests.Sample
{
  internal class SampleSystemBehaviour : EcsSystemBehaviour<SampleState, SampleLocationFinderBehaviour>
  {
    public override SampleState InitState()
    {
      return new SampleState(0);
    }
    
    public override IList<SimulationStep<SampleState>> InitSimulation()
    {
      return new List<SimulationStep<SampleState>>()
      {
        new TranslationModule.StartTranslationsStep(),
        new TranslationModule.CancelTranslationsStep(),
        new TranslationModule.CompleteTranslationsStep()
      };
    }
  }
}