using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Wpf.Helpers.Commands;

namespace WpfApp.Wpf.ViewModels
{
    public class AboutViewModel
    {
        public ICommand OpenAuthorSiteCommand { get; set; }

        public AboutViewModel()
        {
            OpenAuthorSiteCommand = new RelayCommand(OpenAuthorSite);
        }

        public void OpenAuthorSite(object parameter)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://jamesclear.com/") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                // Obsługa błędu, np. brak przeglądarki
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
