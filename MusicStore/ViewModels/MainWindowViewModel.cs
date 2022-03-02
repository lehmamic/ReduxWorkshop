using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using MusicStore.Redux;
using MusicStore.Redux.Actions;
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
                    _dispatcher.Dispatch(new AddAlbumsToLibraryAction(new []{ result.Album }));
                    await result.SaveToDiskAsync();
                }
            });

            this.WhenAnyValue(x => x.Albums.Count)
                .Subscribe(x => CollectionEmpty = x == 0);

            this.WhenActivated(d => d(_state
                .Select(s => s.Library)
                .Subscribe(albums =>
                {
                    foreach (var album in albums.Where(a => !Albums.Any(vm => string.Equals(vm.Title, a.Title, StringComparison.OrdinalIgnoreCase))))
                    {
                        var vm = new AlbumViewModel(album, _albumService);
                        Albums.Add(vm);

                        _ = vm.LoadCover();
                    }
                })));

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
            var albums = await _albumService.LoadCachedAsync();
            _dispatcher.Dispatch(new AddAlbumsToLibraryAction(albums.ToArray()));
        }
    }
}
