using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using MusicStore.Models;
using MusicStore.Services;
using ReactiveUI;

namespace MusicStore.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    private readonly Album _album;
    private readonly IAlbumService _albumService;
    private Bitmap? _cover;

    public AlbumViewModel(Album album, IAlbumService albumService)
    {
        _album = album;
        _albumService = albumService;
    }

    public string Artist => _album.Artist;

    public string Title => _album.Title;

    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    public async Task LoadCover()
    {
        await using (var imageStream = await _albumService.LoadCoverBitmapAsync(_album))
        {
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }
    }

    public async Task SaveToDiskAsync()
    {
        await _albumService.SaveAsync(_album);

        if (Cover != null)
        {
            var bitmap = Cover;

            await Task.Run(() =>
            {
                using (var fs = _albumService.SaveCoverBitmapSteam(_album))
                {
                    bitmap.Save(fs);
                }
            });
        }
    }
}
