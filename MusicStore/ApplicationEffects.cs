using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Proxoft.Redux.Core;
using Proxoft.Redux.Core.Actions;

namespace MusicStore;

public class ApplicationEffects : Effect<ApplicationState>
{
    private IDisposable DoNothingEffect => ActionStream
        .OfType<InitializeEffectsAction>()
        .SelectMany(a => Array.Empty<IAction>())
        .Subscribe(Dispatch);

    protected override IEnumerable<IDisposable> OnConnect()
    {
        yield return DoNothingEffect;
    }
}
