using System;
using System.Reflection;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Redux;
using MusicStore.Services;
using MusicStore.ViewModels;
using MusicStore.Views;
using Proxoft.Redux.Core;
using Proxoft.Redux.Core.Dispatchers;

namespace MusicStore
{
    public partial class App : Application
    {
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            Container = services.BuildServiceProvider();
        }

        public IServiceProvider Container { get; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var store = Container.GetRequiredService<Store<ApplicationState>>();
            store.Initialize(() => new ApplicationState());

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Container.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAlbumService, AlbumService>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddRedux<ApplicationState>(builder =>
            {
                builder.UseReducer<ApplicationReducer>()
                    .AddEffects(Assembly.GetExecutingAssembly())
                    .UseJournaler<ActionJournaler>(a => Console.WriteLine(JsonSerializer.Serialize(a)))
                    .UseExceptionHandler<ApplicationStoreExceptionHandler>();
            });
        }
    }
}
