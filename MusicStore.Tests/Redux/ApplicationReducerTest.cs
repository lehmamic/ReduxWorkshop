using MusicStore.Models;
using MusicStore.Redux;
using MusicStore.Redux.Actions;
using Proxoft.Redux.Core;
using Xunit;

namespace MusicStore.Tests.Redux;

public class ApplicationReducerTest
{
    [Fact]
    public void GivenUnknownAction_WhenReducingTheState_ThenReturnsState()
    {
        // given
        var action = new UnknownAction();
        var state = new ApplicationState();
        var reducer = new ApplicationReducer();

        // when
        var actual = reducer.Reduce(state, action);

        // then
        Assert.Same(state, actual);
        Assert.Empty(actual.SearchResult);
        Assert.Empty(actual.Library);
    }

    [Fact]
    public void GivenSetSearchResultsAction_WhenReducingTheState_ThenSetsTheSearchResults()
    {
        // given
        var action = new SetSearchResultsAction(new[] { new Album("Muse", "Absolution", "http://localhost/absolution") });
        var state = new ApplicationState();
        var reducer = new ApplicationReducer();

        // when
        var actual = reducer.Reduce(state, action);

        // then
        Assert.Collection(
            actual.SearchResult,
            album =>
            {
                Assert.Equal("Muse", album.Artist);
                Assert.Equal("Absolution", album.Title);
                Assert.Equal("http://localhost/absolution", album.CoverUrl);
            });
    }

    [Fact]
    public void GivenSetSearchResultsAction_WhenReducingTheState_ThenReturnsACopyOfTheState()
    {
        // given
        var action = new SetSearchResultsAction(new[] { new Album("Muse", "Absolution", "http://localhost/absolution") });
        var state = new ApplicationState();
        var reducer = new ApplicationReducer();

        // when
        var actual = reducer.Reduce(state, action);

        // then
        Assert.NotSame(state, actual);

    }

    [Fact]
    public void GivenAddAlbumsToLibraryAction_WhenReducingTheState_ThenAddsAlbumsToLibrary()
    {
        // given
        var action = new AddAlbumsToLibraryAction(new[] { new Album("Muse", "Absolution", "http://localhost/absolution") });
        var state = new ApplicationState();
        var reducer = new ApplicationReducer();

        // when
        var actual = reducer.Reduce(state, action);

        // then
        Assert.Collection(
            actual.Library,
            album =>
            {
                Assert.Equal("Muse", album.Artist);
                Assert.Equal("Absolution", album.Title);
                Assert.Equal("http://localhost/absolution", album.CoverUrl);
            });
    }

    [Fact]
    public void GivenAddAlbumsToLibraryAction_WhenReducingTheState_ThenReturnsACopyOfTheState()
    {
        // given
        var action = new AddAlbumsToLibraryAction(new[] { new Album("Muse", "Absolution", "http://localhost/absolution") });
        var state = new ApplicationState();
        var reducer = new ApplicationReducer();

        // when
        var actual = reducer.Reduce(state, action);

        // then
        Assert.NotSame(state, actual);

    }

    private class UnknownAction : IAction
    {
    }
}
