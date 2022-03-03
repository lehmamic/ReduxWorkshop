using System;

namespace MusicStore.Models;

public class Album : IEquatable<Album>
{
    public Album(string artist, string title, string coverUrl)
    {
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
    }

    public string Artist { get; set; }

    public string Title { get; set; }

    public string CoverUrl { get; set; }

    public bool Equals(Album? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Artist == other.Artist && Title == other.Title && CoverUrl == other.CoverUrl;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Album)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Artist, Title, CoverUrl);
    }

    public static bool operator ==(Album? left, Album? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Album? left, Album? right)
    {
        return !Equals(left, right);
    }
}
