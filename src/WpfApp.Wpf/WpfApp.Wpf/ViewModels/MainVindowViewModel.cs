using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp.Core.Interfaces;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;
using WpfApp.Wpf.Views;

namespace WpfApp.Wpf.ViewModels
{
    public class MainVindowViewModel : BaseViewModel
    {

        private object _currentView;
        private NavigationViewItem _selectedMenuItem;



        public ObservableCollection<NavigationViewItem> MenuItems { get; } = new ObservableCollection<NavigationViewItem>();
        public NavigationViewItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set { _selectedMenuItem = value; OnPropertyChanged(nameof(SelectedMenuItem)); }
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }

        public ICommand HamburgerMenuSelectionChangedCommand { get; set; }
        public ICommand ShowWidgetCommand { get; set; }
        private WidgetWindowView? _widget;
        public HomeViewModel _homeViewModel { get; set; }
		public ProgressHistoryViewModel _progressHistoryViewModel { get; }
		public AboutViewModel _aboutViewModel { get; }
		public SettingsViewModel _settingsViewModel { get; }
		public WidgetWindowViewModel _widgetWindowViewModel { get; }
        public MainVindowViewModel(
            HomeViewModel homeViewModel, 
            ProgressHistoryViewModel progressHistoryViewModel,
            AboutViewModel aboutViewModel,
            SettingsViewModel settingsViewModel,
            WidgetWindowViewModel widgetWindowViewModel
            )
        {

            _homeViewModel = homeViewModel;
            _progressHistoryViewModel = progressHistoryViewModel;
            _aboutViewModel = aboutViewModel;
            _settingsViewModel = settingsViewModel;
            _widgetWindowViewModel = widgetWindowViewModel;

            HamburgerMenuSelectionChangedCommand = new RelayCommand(HamburgerMenuSelectionChanged);
            ShowWidgetCommand = new RelayCommand(ShowWidget);

            
            MenuItems.Add(new NavigationViewItem { Content = "Home", Icon = new SymbolIcon(Symbol.Home), Tag = "home" });
            MenuItems.Add(new NavigationViewItem { Content = "About", Icon = new SymbolIcon(Symbol.Help), Tag = "about" });
            SelectedMenuItem = MenuItems[0];
            MenuItems.Add(new NavigationViewItem { Content = "Progres history",
                Icon = new FontIcon
                {
                    Glyph = "\uE9D9", // History
                    FontFamily = new FontFamily("Segoe MDL2 Assets")
                },

                Tag = "history" });

            CurrentView = _homeViewModel;
        }

        public void ShowWidget(object param)
        {
            var widget = new WidgetWindowView
            {
                DataContext = _widgetWindowViewModel
            };

            // ustawiamy widget jako nowe główne okno aplikacji
            Application.Current.MainWindow = widget;

            widget.Show();

            // zamykamy stare główne okno
            Application.Current.Windows
                .OfType<MainVindowView>()
                .FirstOrDefault()
                ?.Hide();
        }
        public void HamburgerMenuSelectionChanged(object param)
        {
            switch (SelectedMenuItem.Tag)
            {
                case "home":
                    CurrentView = _homeViewModel;
                    break;
                case "history":
                    CurrentView = _progressHistoryViewModel;
                    break;
                case "about":
                    CurrentView = _aboutViewModel;
                    break;
                case "Ustawienia":
                    CurrentView = _settingsViewModel;
                    break;

            }
        }
    }
}
