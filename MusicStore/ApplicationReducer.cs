using Proxoft.Redux.Core;

namespace MusicStore;

public class ApplicationReducer : IReducer<ApplicationState>
{
    public ApplicationState Reduce(ApplicationState state, IAction action)
    {
        return state;
    }
}
