using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WpfApp.Core.Data;
using WpfApp.Wpf.ViewModels;
using WpfApp.Wpf.Views;
using System;

namespace WpfApp.Wpf.Bootstrapper
{
    public class Bootstrapper
    {
        private static IServiceProvider _serviceProvider;
        public ServiceProvider ServiceProvider { get; private set; }

        public void Run()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Run main window
            var mainWindow = ServiceProvider.GetRequiredService<MainVindowView>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Konfiguracja bazy danych SQLite
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=calculations.db"));

            // Business serwises


            // ViewModels
            services.AddTransient<HomeViewModel>();
            services.AddTransient<MainVindowViewModel>();

            // Views
            services.AddTransient<HomeView>(provider => new HomeView
            {
                DataContext = provider.GetRequiredService<HomeViewModel>()
            });

            services.AddTransient<MainVindowView>(provider => new MainVindowView
            {
                DataContext = provider.GetRequiredService<MainVindowViewModel>()
            });
        }
    }
}
