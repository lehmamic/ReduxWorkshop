using Proxoft.Redux.Core;

namespace MusicStore.Redux.Actions;

public class SearchAlbumsAction : IAction
{
    public string? Term { get; }

    public SearchAlbumsAction(string? term)
    {
        Term = term;
    }
}
