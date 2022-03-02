using System.Collections.Generic;
using System.Reactive.Linq;
using MarbleTest.Net;
using Moq;
using Moq.AutoMock;
using MusicStore.Models;
using MusicStore.Redux;
using MusicStore.Redux.Actions;
using MusicStore.Services;
using Proxoft.Redux.Core;
using Xunit;

namespace MusicStore.Tests.Redux;

public class ApplicationEffectsTest
{
    private readonly AutoMocker _mocker = new();
    private readonly MarbleScheduler _scheduler = new();

    [Fact]
    public void GivenSearchAlbumsAction_WhenAsyncResultArrives_ReturnsCorrectActions()
    {
        // given
        var state = new ApplicationState();
        var action = new SearchAlbumsAction("any term");

        var effect = _mocker.CreateInstance<ApplicationEffects>();

        _mocker.GetMock<IAlbumService>()
            .Setup(s => s.SearchAsync(It.IsAny<string>()))
            .Returns(()=>
            {
                return Observable.Return<IEnumerable<Album>>(new[] { new Album("Muse", "Absolution", "http://localhost/absolution") });
            });

        // when
        var sourceEvents = _scheduler.CreateColdObservable<StateActionPair<ApplicationState>>(
            "--a-",
            new
            {
                a = new StateActionPair<ApplicationState>(state, action),
            });
        effect.Connect(sourceEvents);

        // then
        _scheduler.ExpectObservable(effect.SearchAlbums)
            .ToBe(
                "--(abc)-",
                new
                {
                    a = new SetBusyAction(true),
                    b = new SetBusyAction(false),
                    c = new SetSearchResultsAction(new[] { new Album("Muse", "Absolution", "http://localhost/absolution") }),
                });
        _scheduler.Flush();
    }
}
