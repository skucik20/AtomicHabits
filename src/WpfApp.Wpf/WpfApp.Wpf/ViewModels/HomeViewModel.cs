using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;

namespace WpfApp.Wpf.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand CreateAtomicHabitCommand { get; set; }
        public HomeViewModel()
        {
            CreateAtomicHabitCommand = new RelayCommand(CreateAtomicHabit);
        }

        private void CreateAtomicHabit(object parameter)
        {
            MessageBox.Show("AA");
        }
    }
}
