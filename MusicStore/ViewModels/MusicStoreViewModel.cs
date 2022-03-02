using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using MusicStore.Redux;
using MusicStore.Redux.Actions;
using MusicStore.Services;
using Proxoft.Redux.Core;
using ReactiveUI;

namespace MusicStore.ViewModels;

public class MusicStoreViewModel : ViewModelBase
{
    private readonly IAlbumService _albumService;
    private readonly IActionDispatcher _dispatcher;
    private readonly IStateStream<ApplicationState> _store;
    private bool _isBusy;
    private string? _searchText;
    private AlbumViewModel? _selectedAlbum;
    private CancellationTokenSource? _cancellationTokenSource;

    public MusicStoreViewModel(IAlbumService albumService, IActionDispatcher dispatcher, IStateStream<ApplicationState> store)
    {
        _albumService = albumService;
        _dispatcher = dispatcher;
        _store = store;

        BuyMusicCommand = ReactiveCommand.Create(() =>
        {
            return SelectedAlbum;
        });

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(400))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(DoSearch!);

            this.WhenActivated(d => d(_store
                .Select(s => s.SearchResult)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(albums =>
                {
                    _cancellationTokenSource?.Cancel();
                    _cancellationTokenSource = new CancellationTokenSource();

                    SearchResults.Clear();

                    foreach (var album in albums)
                    {
                        var vm = new AlbumViewModel(album, _albumService);
                        SearchResults.Add(vm);
                    }

                    var cancellationToken = _cancellationTokenSource.Token;
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        LoadCovers(cancellationToken);
                    }
                })));
    }

    public ReactiveCommand<Unit, AlbumViewModel?> BuyMusicCommand { get; }

    public string? SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();

    public AlbumViewModel? SelectedAlbum
    {
        get => _selectedAlbum;
        set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
    }

    private async void DoSearch(string s)
    {
        IsBusy = true;

        if (!string.IsNullOrWhiteSpace(s))
        {
            var albums = await _albumService.SearchAsync(s);
            _dispatcher.Dispatch(new SetSearchResultsAction(albums.ToArray()));
        }

        IsBusy = false;
    }

    private async void LoadCovers(CancellationToken cancellationToken)
    {
        foreach (var album in SearchResults.ToList())
        {
            await album.LoadCover();

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }
}
