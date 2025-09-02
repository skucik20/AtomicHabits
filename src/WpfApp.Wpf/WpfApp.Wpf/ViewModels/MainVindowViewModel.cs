using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Wpf.ViewModels.Shared;

namespace WpfApp.Wpf.ViewModels
{
    public class MainVindowViewModel : BaseViewModel
    {
		public HomeViewModel HomeViewModel { get; }
        public MainVindowViewModel(HomeViewModel homeViewModel)
        {
            HomeViewModel = homeViewModel;
        }
    }
}
