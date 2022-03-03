using System;
using System.Linq;
using MusicStore.Models;
using Proxoft.Redux.Core;

namespace MusicStore.Redux.Actions;

public class SetSearchResultsAction : IAction, IEquatable<SetSearchResultsAction>
{
    public SetSearchResultsAction(Album[] searchResults)
    {
        SearchResults = searchResults;
    }

    public Album[] SearchResults { get; }

    public bool Equals(SetSearchResultsAction? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return SearchResults.SequenceEqual(other.SearchResults);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SetSearchResultsAction)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = default(HashCode);

        foreach (var action in SearchResults)
        {
            hashCode.Add(action);
        }

        return hashCode.ToHashCode();
    }

    public static bool operator ==(SetSearchResultsAction? left, SetSearchResultsAction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SetSearchResultsAction? left, SetSearchResultsAction? right)
    {
        return !Equals(left, right);
    }
}
