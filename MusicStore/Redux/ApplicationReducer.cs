using System.Collections.Immutable;
using System.Linq;
using MusicStore.Redux.Actions;
using Proxoft.Redux.Core;

namespace MusicStore.Redux;

public class ApplicationReducer : IReducer<ApplicationState>
{
    public ApplicationState Reduce(ApplicationState state, IAction action)
    {
        return action switch
        {
            SetSearchResultsAction a => state with { SearchResult = a.SearchResults.ToImmutableArray() },
            AddAlbumsToLibraryAction a => state with { Library = state.Library.Concat(a.Albums).ToImmutableArray() },
            SetBusyAction a => state with { IsBusy = a.IsBusy },
            _ => state
        };
    }
}
