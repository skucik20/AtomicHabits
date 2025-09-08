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

            // close widget
            Application.Current.Windows
                .OfType<WidgetWindowView>()
                .FirstOrDefault()
                ?.Close();


            // unhide main window
            //TODO is main window Application.Current.MainWindow
            Application.Current.Windows
                .OfType<MainVindowView>()
                .FirstOrDefault()
                ?.Show();
        }
    }
}
