using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Services;
using WpfApp.Wpf.Helpers;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;

namespace WpfApp.Wpf.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private string _habitTitle = string.Empty;
        private string _habitDescription = string.Empty;
        private bool _isHabitChecked;
        public ICommand AddHabitCommand { get; set; }

        private readonly IAtomicHabitService _atomicHabitService;
        private readonly IProgressHistoryService _progressHistoryService;

        private ObservableCollection<AtomicHabitModel> _atimicHabitsCollection;

        #region Properties
        public string HabitTitle
        {
            get { return _habitTitle; }
            set { _habitTitle = value; }
        }

        public string HabitDescription
        {
            get { return _habitDescription; }
            set { _habitDescription = value; }
        }
        public bool IsHabitChecked
        {
            get { return _isHabitChecked; }
            set { _isHabitChecked = value; OnPropertyChanged(nameof(IsHabitChecked)); }
        }
        #endregion
        public ObservableCollection<AtomicHabitModel> AtimicHabitsCollection
        {
            get { return _atimicHabitsCollection; }
            set { _atimicHabitsCollection = value; }
        }

        public HomeViewModel(IAtomicHabitService atomicHabitService, IProgressHistoryService progressHistoryService)
        {
            AtimicHabitsCollection = new ObservableCollection<AtomicHabitModel>();
            
            _atomicHabitService = atomicHabitService;
            _progressHistoryService = progressHistoryService;

            AddHabitCommand = new RelayCommandAsync(AddHabit);

            _ = _atomicHabitService.HasTodayAtomicHabitChecked();
            _ = LoadData();   
        }

        public async Task AddHabit(object parametr)
        {
            await _atomicHabitService.AddAsync(new AtomicHabitModel { Title = HabitTitle, Description = HabitDescription });
            _ = LoadData();
        }

        private async Task LoadData()
        {
            var atomicHabits = await _atomicHabitService.GetAllAsync();
            AtimicHabitsCollection.Clear();
            foreach (var p in atomicHabits)
            {
                p.PropertyChanged += AtomicHabit_PropertyChanged;
                AtimicHabitsCollection.Add(p);
            }
                
        }

        private async void AtomicHabit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AtomicHabitModel.IsHabitDone))
            {
                var atomicHabit = (AtomicHabitModel)sender;
                await _progressHistoryService.AddProgressAsync(atomicHabit);
                await _atomicHabitService.UpdateAsync(atomicHabit);
            }
        }

    }
}
