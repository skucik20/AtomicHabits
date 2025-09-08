using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;
using WpfApp.Wpf.Views;
using System.Windows;

namespace WpfApp.Wpf.ViewModels
{
    public class WidgetWindowViewModel : BaseViewModel
    {
        public ICommand ShowMainWindowCommand { get; set; }
        public WidgetWindowViewModel()
        {
            ShowMainWindowCommand = new RelayCommand(ShowMainWindow);
        }

        private void ShowMainWindow(object parameter)
        {
            //var widget = new WidgetWindowView
            //{
            //    DataContext = this
            //};

            //// ustawiamy widget jako nowe główne okno aplikacji
            //Application.Current.MainWindow = widget;

            //widget.Show();

            // zamykamy stare główne okno
            Application.Current.Windows
                .OfType<WidgetWindowView>()
                .FirstOrDefault()
                ?.Close();


            // zamykamy stare główne okno
            Application.Current.Windows
                .OfType<MainVindowView>()
                .FirstOrDefault()
                ?.Show();
        }
    }
}
