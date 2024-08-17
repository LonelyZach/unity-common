using System.Collections.Generic;
using System.Linq;
using com.lonely.common.CommonUtil;
using com.lonely.common.EcsSystem;
using Tests.RpgSystemTests.Sample.Components;
using Tests.RpgSystemTests.Sample.Modules;

namespace Tests.RpgSystemTests.Sample.Statuses
{
  internal class Locked : Status, IEcsObserver<SampleState>
  {
    public override string Name => nameof(Locked);

    public IEnumerable<IEcsObserver<SampleState>.Register> Observers()
    {
      yield return IEcsObserver<SampleState>.On<TranslationModule.TranslationStarted>(
        (evt) => evt.Teleport,
        (state, evt) =>
        {
          state.Characters
            .Where(x => x.Id.Value == evt.Entity)
            .SelectManyNotNull(x => x.Components.Get<Stack>().Where(s => s.Status.Name == Name).Select(y => (Character: x, Stack: y)), out var operations);

          foreach (var (character, stack) in operations)
          {
            state.Cmds.Add(new TranslationModule.CancelTranslations(character.Id.Value));
          }
        });
    }
  }
}