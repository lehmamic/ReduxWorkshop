using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MusicStore.Redux.Actions;
using MusicStore.Services;
using MusicStore.Utils;
using Proxoft.Redux.Core;

namespace MusicStore.Redux;

public class ApplicationEffects : Effect<ApplicationState>
{
    private readonly ISchedulerProvider _schedulerProvider;
    private readonly IAlbumService _albumService;

    public ApplicationEffects(ISchedulerProvider schedulerProvider, IAlbumService albumService)
    {
        _schedulerProvider = schedulerProvider;
        _albumService = albumService;
    }

    public IObservable<IAction> SearchAlbums => ActionStream
        .OfType<SearchAlbumsAction>()
        .Throttle(_schedulerProvider.CreateTime(TimeSpan.FromMilliseconds(400)), _schedulerProvider.Scheduler)
        .Select(a => _albumService.SearchAsync(a.Term)
            .SelectMany(searchResults => new IAction[]
            {
                new SetBusyAction(false),
                new SetSearchResultsAction(searchResults.ToArray())
            })
            .StartWith(new SetBusyAction(true))
        )
        .Switch();

    protected override IEnumerable<IDisposable> OnConnect()
    {
        yield return SearchAlbums
            .Subscribe(Dispatch);
    }
}
