using MusicStore.Models;
using Proxoft.Redux.Core;

namespace MusicStore.Redux.Actions;

public class SetSearchResultsAction : IAction
{
    public SetSearchResultsAction(Album[] searchResults)
    {
        SearchResults = searchResults;
    }

    public Album[] SearchResults { get; }
}
