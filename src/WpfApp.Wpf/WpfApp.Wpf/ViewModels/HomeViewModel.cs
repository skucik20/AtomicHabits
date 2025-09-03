using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;

namespace WpfApp.Wpf.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IAtomicHabitService _atomicHabitService;
        public ICommand CreateAtomicHabitCommand { get; set; }
        private ObservableCollection<AtomicHabitModel> _atimicHabitsCollection;



        public ObservableCollection<AtomicHabitModel> AtimicHabitsCollection
        {
            get { return _atimicHabitsCollection; }
            set { _atimicHabitsCollection = value; }
        }

        public HomeViewModel(IAtomicHabitService atomicHabitService)
        {
            AtimicHabitsCollection = new ObservableCollection<AtomicHabitModel>();
            _atomicHabitService = atomicHabitService;
            _ = LoadData();
            CreateAtomicHabitCommand = new RelayCommand(CreateAtomicHabit);
        }

        private void CreateAtomicHabit(object parameter)
        {
            MessageBox.Show("AA");
        }

        private async Task LoadData()
        {
            var atomicHabits = await _atomicHabitService.GetAllAsync();
            AtimicHabitsCollection.Clear();
            foreach (var p in atomicHabits)
                AtimicHabitsCollection.Add(p);
        }
    }
}
