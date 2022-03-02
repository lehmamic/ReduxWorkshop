using System;
using System.Collections.Immutable;
using MusicStore.Models;

namespace MusicStore.Redux;

public record ApplicationState
{
    public IImmutableList<Album> SearchResult { get; init; } = Array.Empty<Album>().ToImmutableArray();

    public IImmutableList<Album> Library { get; init; } = Array.Empty<Album>().ToImmutableArray();
}
