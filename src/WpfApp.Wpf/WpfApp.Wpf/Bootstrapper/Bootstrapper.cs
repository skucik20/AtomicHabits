using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WpfApp.Core.Data;
using WpfApp.Wpf.ViewModels;
using WpfApp.Wpf.Views;
using System;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Services;
using System.IO;
using WpfApp.Core.Constans;
using WpfApp.Wpf.Helpers;

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

            var dailyResetService = ServiceProvider.GetRequiredService<IDailyResetService>();
            dailyResetService.Start();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            //var dbPath = Path.Combine(AppContext.BaseDirectory, "app.db");
            var dbPath = DbConnectionString.connectionString;
            // SQLite database configuration
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));


            // Business serwises
            services.AddScoped<IAtomicHabitService, AtomicHabitService>();
            services.AddSingleton<IDailyResetService, DailyResetService>();
            services.AddSingleton<IProgressHistoryService, ProgressHistoryService>();

            // ViewModels
            services.AddTransient<HomeViewModel>();
            services.AddTransient<CreateAtomicHabitViewModel>();
            services.AddTransient<MainVindowViewModel>();
            services.AddTransient<ProgressHistoryViewModel>();

            // Views
            services.AddTransient<ProgressHistoryView>(provider => new ProgressHistoryView
            {
                DataContext = provider.GetRequiredService<ProgressHistoryViewModel>()
            });
            services.AddTransient<CreateAtomicHabitView>(provider => new CreateAtomicHabitView
            {
                DataContext = provider.GetRequiredService<CreateAtomicHabitViewModel>()
            });
            
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
