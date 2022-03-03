using System;
using Proxoft.Redux.Core;

namespace MusicStore.Redux.Actions;

public class SetBusyAction : IAction, IEquatable<SetBusyAction>
{
    public bool IsBusy { get; }

    public SetBusyAction(bool isBusy)
    {
        IsBusy = isBusy;
    }

    public bool Equals(SetBusyAction? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return IsBusy == other.IsBusy;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SetBusyAction)obj);
    }

    public override int GetHashCode()
    {
        return IsBusy.GetHashCode();
    }

    public static bool operator ==(SetBusyAction? left, SetBusyAction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SetBusyAction? left, SetBusyAction? right)
    {
        return !Equals(left, right);
    }
}
