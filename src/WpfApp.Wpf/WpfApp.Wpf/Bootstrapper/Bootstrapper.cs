using Microsoft.Extensions.DependencyInjection;

using WpfApp.Wpf.ViewModels;
using WpfApp.Wpf.Views;

namespace WpfApp.Wpf.Bootstrapper
{
    public class Bootstrapper
    {
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
            // Serwisy biznesowe


            // ViewModele
            services.AddTransient<MainVindowViewModel>();

            // Okna
            services.AddTransient<MainVindowView>(provider => new MainVindowView
            {
                DataContext = provider.GetRequiredService<MainVindowViewModel>()
            });
        }
    }
}
