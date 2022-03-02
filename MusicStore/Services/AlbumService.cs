using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using iTunesSearch.Library;
using MusicStore.Models;

namespace MusicStore.Services;

public class AlbumService : IAlbumService
{
    private const string CacheRoot = "./.cache";

    private static HttpClient _httpClient = new();
    private static iTunesSearchManager _searchManager = new();

    public async Task<Stream> LoadCoverBitmapAsync(Album album)
    {
        if (File.Exists(GetCachePath(album) + ".bmp"))
        {
            return File.OpenRead(GetCachePath(album) + ".bmp");
        }
        else
        {
            var data = await _httpClient.GetByteArrayAsync(album.CoverUrl);

            return new MemoryStream(data);
        }
    }

    public async Task SaveAsync(Album album)
    {
        if (!Directory.Exists(CacheRoot))
        {
            Directory.CreateDirectory(CacheRoot);
        }

        using (var fs = File.OpenWrite(GetCachePath(album)))
        {
            await SaveToStreamAsync(album, fs);
        }
    }

    public Stream SaveCoverBitmapSteam(Album album)
    {
        return File.OpenWrite(GetCachePath(album) + ".bmp");
    }

    public async Task<Album> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
    }

    public async Task<IEnumerable<Album>> LoadCachedAsync()
    {
        if (!Directory.Exists(CacheRoot))
        {
            Directory.CreateDirectory(CacheRoot);
        }

        var results = new List<Album>();

        foreach (var file in Directory.EnumerateFiles(CacheRoot))
        {
            if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;

            await using var fs = File.OpenRead(file);
            results.Add(await LoadFromStream(fs).ConfigureAwait(false));
        }

        return results;
    }

    public IObservable<IEnumerable<Album>> SearchAsync(string? searchTerm)
    {
        return Observable.FromAsync(async () =>
        {
            var query = await _searchManager.GetAlbumsAsync(searchTerm).ConfigureAwait(false);

            return query.Albums.Select(x =>
                new Album(x.ArtistName, x.CollectionName, x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
        });
    }

    private string GetCachePath(Album album) => $"{CacheRoot}/{album.Artist} - {album.Title}";

    private async Task SaveToStreamAsync(Album data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
    }
}
