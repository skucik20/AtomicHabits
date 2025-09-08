using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Services;
using WpfApp.Wpf.Helpers;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;
using WpfApp.Wpf.Views;

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

        public ICommand ShowMainWindowCommand { get; set; }


        private readonly IAtomicHabitService _atomicHabitService;
        private readonly IProgressHistoryService _progressHistoryService;

        public ObservableCollection<AtomicHabitModel> AtimicHabitsCollection { get; set; }

        #region Properties
        private AtomicHabitModel _currentAtomicHabit;

        public AtomicHabitModel CurrentAtomicHabit
        {
            get { return _currentAtomicHabit; }
            set { _currentAtomicHabit = value; OnPropertyChanged(nameof(CurrentAtomicHabit)); }
        }

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


        private int _currentIndex = 0;
        private DispatcherTimer _timer;


        public HomeViewModel(IAtomicHabitService atomicHabitService, IProgressHistoryService progressHistoryService)
        {

            AtimicHabitsCollection = new ObservableCollection<AtomicHabitModel>();
            
            _atomicHabitService = atomicHabitService;
            _progressHistoryService = progressHistoryService;

            AddHabitCommand = new RelayCommandAsync(AddHabit);
            ClearHabitCommand = new RelayCommand(ClearHabit);
            ShowMainWindowCommand = new RelayCommand(ShowMainWindow);

            HabitDelateCommand = new RelayCommand<AtomicHabitModel>(HabitDelate);
            HabitEditCommand = new RelayCommand<AtomicHabitModel>(HabitEdit);



            _ = _atomicHabitService.HasTodayAtomicHabitChecked();
            _ = LoadData();

            if (AtimicHabitsCollection.Count > 0)
                CurrentAtomicHabit = AtimicHabitsCollection[0];

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ObservableCollection<AtomicHabitModel> falseCollection = new ObservableCollection<AtomicHabitModel>();

            foreach(var habit in AtimicHabitsCollection)
            {
                if(habit.IsHabitDone == false)
                {
                    falseCollection.Add(habit);
                }
            }

            if (falseCollection.Count == 0) return;

            _currentIndex++;
            if (_currentIndex >= falseCollection.Count)
                _currentIndex = 0;

            
            CurrentAtomicHabit = falseCollection[_currentIndex];
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
            var toast = new ToastWindow("Sukces!", "Twoja operacja zakończyła się pomyślnie.");
            toast.Show();
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
