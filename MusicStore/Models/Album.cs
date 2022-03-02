namespace MusicStore.Models;

public class Album
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
}
