using Proxoft.Redux.Core;

namespace MusicStore.Redux.Actions;

public class SetBusyAction : IAction
{
    public bool IsBusy { get; }

    public SetBusyAction(bool isBusy)
    {
        IsBusy = isBusy;
    }
}
