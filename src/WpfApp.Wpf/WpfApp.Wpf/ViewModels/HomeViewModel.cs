using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public ICommand ClearHabitCommand { get; set; }
        public ICommand HabitDelateCommand { get; }
        public ICommand HabitEditCommand { get; set; }


        private readonly IAtomicHabitService _atomicHabitService;
        private readonly IProgressHistoryService _progressHistoryService;

        private ObservableCollection<AtomicHabitModel> _atimicHabitsCollection;

        #region Properties
        public string HabitTitle
        {
            get { return _habitTitle; }
            set { _habitTitle = value; OnPropertyChanged(nameof(HabitTitle)); }
        }

        public string HabitDescription
        {
            get { return _habitDescription; }
            set { _habitDescription = value; OnPropertyChanged(nameof(HabitDescription)); }
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
            ClearHabitCommand = new RelayCommand(ClearHabit);

            HabitDelateCommand = new RelayCommand<AtomicHabitModel>(HabitDelate);
            HabitEditCommand = new RelayCommand<AtomicHabitModel>(HabitEdit);



            _ = _atomicHabitService.HasTodayAtomicHabitChecked();
            _ = LoadData();   
        }

        private void HabitEdit(AtomicHabitModel atomicHabit)
        {
            switch (atomicHabit.IsEditHabitModeOn)
            {
                case true:
                    SetEditToggle(atomicHabit, false);
                    break;
                case false:
                    SetEditToggle(atomicHabit, true);
                    break;
            }
        }


        private void SetEditToggle(AtomicHabitModel atomicHabit, bool setTo)
        {
            foreach (var item in AtimicHabitsCollection)
            {
                if (atomicHabit.Id == item.Id)
                    item.IsTextBlockReadOnly = setTo;
            }
        }

        private void ClearHabit(object parameter)
        {
            HabitTitle = string.Empty;
            HabitDescription = string.Empty;
        }
        private void HabitDelate(AtomicHabitModel habit)
        {
            _atomicHabitService.DeleteAsync(habit.Id);
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
            var atomicHabit = (AtomicHabitModel)sender;
            await _progressHistoryService.AddProgressAsync(atomicHabit);
            await _atomicHabitService.UpdateAsync(atomicHabit);
        }

    }
}
