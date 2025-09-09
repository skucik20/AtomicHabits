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
using WpfApp.Core.Enums;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Models.Shared;
using WpfApp.Core.Services;
using WpfApp.Wpf.Helpers;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;
using WpfApp.Wpf.Views;

namespace WpfApp.Wpf.ViewModels
{

    public class CategoriesComboBox : BaseComboBoxModel
    {
        public CategoryModel Category { get; set; }
    }
    public class HomeViewModel : BaseViewModel
    {
        private CategoriesComboBox _selectedCategoriesComboBoxItem;
        private AtomicHabitModel _currentAtomicHabit;
        private string _habitTitle = string.Empty;
        private string _habitDescription = string.Empty;
        private bool _isHabitChecked;
        private bool _isHabitListEmpty;
        public ICommand AddHabitCommand { get; set; }
        public ICommand ClearHabitCommand { get; set; }
        public ICommand HabitDelateCommand { get; }
        public ICommand HabitEditCommand { get; set; }
        public ICommand ShowMainWindowCommand { get; set; }



        private readonly IAtomicHabitService _atomicHabitService;
        private readonly IProgressHistoryService _progressHistoryService;
        private readonly ICategoryService _categoryService;

        public ObservableCollection<AtomicHabitModel> AtimicHabitsCollection { get; set; }
        public ObservableCollection<CategoriesComboBox> CategoriesComboBoxCollection { get; set; } = new ObservableCollection<CategoriesComboBox>();

        #region Properties

        public CategoriesComboBox SelectedCategoriesComboBoxItem
        {
            get { return _selectedCategoriesComboBoxItem; }
            set { _selectedCategoriesComboBoxItem = value; OnPropertyChanged(nameof(SelectedCategoriesComboBoxItem)); }
        }

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
        public bool IsHabitListEmpty
        {
            get { return _isHabitListEmpty; }
            set { _isHabitListEmpty = value; OnPropertyChanged(nameof(IsHabitListEmpty)); }
        }
        #endregion


        private int _currentIndex = 0;
        private DispatcherTimer _timer;


        public HomeViewModel(IAtomicHabitService atomicHabitService, IProgressHistoryService progressHistoryService, ICategoryService categoryService)
        {

            AtimicHabitsCollection = new ObservableCollection<AtomicHabitModel>();
            
            _atomicHabitService = atomicHabitService;
            _progressHistoryService = progressHistoryService;
            _categoryService = categoryService;

            AddHabitCommand = new RelayCommandAsync(AddHabit);
            ClearHabitCommand = new RelayCommand(ClearHabit);
            ShowMainWindowCommand = new RelayCommand(ShowMainWindow);

            HabitDelateCommand = new RelayCommand<AtomicHabitModel>(HabitDelate);
            HabitEditCommand = new RelayCommand<AtomicHabitModel>(HabitEdit);



            _ = _atomicHabitService.HasTodayAtomicHabitChecked();
            _ = LoadData();
            _ = LoadCategoryCombobox();
            SelectedCategoriesComboBoxItem = CategoriesComboBoxCollection[0];

            widgetHabtisChanger();
            IsAtomicHabitsListEmpty();
        }

        private void IsAtomicHabitsListEmpty()
        {
            if(AtimicHabitsCollection.Count() == 0)
            {
                IsHabitListEmpty = false;
            }
            else
            {
                IsHabitListEmpty = true;
            }
        }

        private void widgetHabtisChanger()
        {
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
            try {
                Application.Current.Windows
                .OfType<WidgetWindowView>()
                .FirstOrDefault()
                ?.Close();
            }
            catch { }
            

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
        private async void HabitDelate(AtomicHabitModel habit)
        {
            var dialog = new ContentDialog
            {
                Title = "Note",
                Content = "Are you sure you want to delate habit",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                _atomicHabitService.DeleteAsync(habit.Id);
                _ = LoadData();
            }
            IsAtomicHabitsListEmpty();
        }

        public async Task AddHabit(object parametr)
        {
            eHabitTitleState habitTitleState = await _atomicHabitService.AddAsync(new AtomicHabitModel
            {
                Title = HabitTitle,
                Description = HabitDescription,
                CategoryId = SelectedCategoriesComboBoxItem.Id,
                CategoryModel = SelectedCategoriesComboBoxItem.Category

            });
            _ = LoadData();
            
            
            switch(habitTitleState)
            {
                case eHabitTitleState.duplicated:
                    var dialog = new ContentDialog
                    {
                        Title = "Note",
                        Content = "You cannot add a habit with the same title.",
                        CloseButtonText = "Ok"
                    };
                    await dialog.ShowAsync();

                    break;

                case eHabitTitleState.empty:
                    dialog = new ContentDialog
                    {
                        Title = "Note",
                        Content = "You cannot add an empty habit.",
                        CloseButtonText = "Ok"
                    };
                    await dialog.ShowAsync();

                    break;
                case eHabitTitleState.unique:

                    var toast = new ToastWindow("Success!", "Your operation completed successfully.");
                    toast.Show();
                    break;
            }


            
            IsAtomicHabitsListEmpty();
        }

        private async Task LoadCategoryCombobox()
        {
            var categories = await _categoryService.GetAllAsync();
            CategoriesComboBoxCollection.Clear();
            foreach (var category in categories)
            {
                CategoriesComboBoxCollection.Add(
                    new CategoriesComboBox
                    {
                        Id = category.Id,
                        Content = category.CategoryName,
                        Category = category
                    }
                );
            }
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
