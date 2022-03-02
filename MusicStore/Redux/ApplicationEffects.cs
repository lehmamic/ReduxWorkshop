using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MusicStore.Redux.Actions;
using MusicStore.Services;
using Proxoft.Redux.Core;
using Proxoft.Redux.Core.Actions;
using Proxoft.Redux.Core.Extensions;

namespace MusicStore.Redux;

public class ApplicationEffects : Effect<ApplicationState>
{
    private readonly IAlbumService _albumService;

    public ApplicationEffects(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    private IDisposable SearchAlbums => ActionStream
        .OfType<SearchAlbumsAction>()
        .Throttle(TimeSpan.FromMilliseconds(400))
        .Select(a => Observable.FromAsync(() => _albumService.SearchAsync(a.Term))
            .SelectMany(searchResults => new IAction[]
            {
                new SetBusyAction(false),
                new SetSearchResultsAction(searchResults.ToArray())
            })
            .StartWith(new SetBusyAction(true))
        )
        .Switch()
        .Subscribe(Dispatch);

    protected override IEnumerable<IDisposable> OnConnect()
    {
        yield return SearchAlbums;
    }
}
