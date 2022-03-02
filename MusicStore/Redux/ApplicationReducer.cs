using System.Collections.Immutable;
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
            _ => state
        };
    }
}
