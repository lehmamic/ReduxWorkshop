using MusicStore.Models;
using Proxoft.Redux.Core;

namespace MusicStore.Redux.Actions;

public class AddAlbumsToLibraryAction : IAction
{
    public AddAlbumsToLibraryAction(Album[] albums)
    {
        Albums = albums;
    }

    public Album[] Albums { get; }
}
