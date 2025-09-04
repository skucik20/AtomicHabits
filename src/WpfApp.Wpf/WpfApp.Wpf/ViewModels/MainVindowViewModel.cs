using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            set { _selectedMenuItem = value; }
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

        public HomeViewModel _homeViewModel { get; set; }
		public ProgressHistoryViewModel _progressHistoryViewModel { get; }
        public MainVindowViewModel(HomeViewModel homeViewModel, ProgressHistoryViewModel progressHistoryViewModel)
        {

            _homeViewModel = homeViewModel;
            _progressHistoryViewModel = progressHistoryViewModel;

            HamburgerMenuSelectionChangedCommand = new RelayCommand(HamburgerMenuSelectionChanged);

            
            MenuItems.Add(new NavigationViewItem { Content = "Home", Icon = new SymbolIcon(Symbol.Home), Tag = "home" });
            MenuItems.Add(new NavigationViewItem { Content = "Progres history",
                Icon = new FontIcon
                {
                    Glyph = "\uE9D9", // History
                    FontFamily = new FontFamily("Segoe MDL2 Assets")
                },

                Tag = "history" });
            MenuItems.Add(new NavigationViewItem { Content = "About", Icon = new SymbolIcon(Symbol.Help), Tag = "about" });
            SelectedMenuItem = MenuItems[0];

            CurrentView = _homeViewModel;
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
                case "Ustawienia":
                    CurrentView = _progressHistoryViewModel;
                    break;

            }
        }
    }
}
