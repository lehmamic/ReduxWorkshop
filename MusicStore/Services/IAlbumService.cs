using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MusicStore.Models;

namespace MusicStore.Services;

public interface IAlbumService
{
    Task<Stream> LoadCoverBitmapAsync(Album album);

    Task SaveAsync(Album album);

    Stream SaveCoverBitmapSteam(Album album);

    Task<Album> LoadFromStream(Stream stream);

    Task<IEnumerable<Album>> LoadCachedAsync();

    Task<IEnumerable<Album>> SearchAsync(string searchTerm);
}
