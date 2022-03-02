using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using MusicStore.Redux;
using MusicStore.Services;
using Proxoft.Redux.Core;
using ReactiveUI;

namespace MusicStore.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IAlbumService _albumService;
        private readonly IActionDispatcher _dispatcher;
        private readonly IStateStream<ApplicationState> _state;
        private bool _collectionEmpty;

        public MainWindowViewModel(IAlbumService albumService, IActionDispatcher dispatcher, IStateStream<ApplicationState> state)
        {
            _albumService = albumService;
            _dispatcher = dispatcher;
            _state = state;

            ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();

            BuyMusicCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var store = new MusicStoreViewModel(albumService, dispatcher, state);

                var result = await ShowDialog.Handle(store);
                if (result != null)
                {
                    Albums.Add(result);

                    await result.SaveToDiskAsync();
                }
            });

            this.WhenAnyValue(x => x.Albums.Count)
                .Subscribe(x => CollectionEmpty = x == 0);

            RxApp.MainThreadScheduler.Schedule(LoadAlbums);
        }

        public ICommand BuyMusicCommand { get; }

        public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }

        public bool CollectionEmpty
        {
            get => _collectionEmpty;
            set => this.RaiseAndSetIfChanged(ref _collectionEmpty, value);
        }

        public ObservableCollection<AlbumViewModel> Albums { get; } = new();

        private async void LoadAlbums()
        {
            var albums = (await _albumService.LoadCachedAsync()).Select(x => new AlbumViewModel(x, _albumService));

            foreach (var album in albums)
            {
                Albums.Add(album);
            }

            foreach (var album in Albums.ToList())
            {
                await album.LoadCover();
            }
        }

    }
}
