using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Models.Shared;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;

namespace WpfApp.Wpf.ViewModels
{
    public class HabitHistorySelectionComboBoxModel : BaseComboBoxModel
    {   
        public ObservableCollection<ProgressHistoryModel> ProgressCollection { get; set; }
        public ObservableCollection<DateTime> SelectedDates { get; set; }
    }
    public class ProgressHistoryViewModel :BaseViewModel
    {

        private ObservableCollection<AtomicHabitModel> _atimicHabitsCollection;
        private Dictionary<int, ObservableCollection<ProgressHistoryModel>> _progressByHabit;
        private HabitHistorySelectionComboBoxModel _seledtedHabitHistorySelectionComboBox;
        private ObservableCollection<ProgressHistoryModel> _progressCollection;
        private DateTime _dateToEdit = DateTime.Today;
        private bool _isHabitDoneToEdit;

        public ObservableCollection<HabitHistorySelectionComboBoxModel> HabitHistorySelectionComboBox { get; set; }

        




        #region properties
        public ObservableCollection<AtomicHabitModel> AtimicHabitsCollection
        {
            get { return _atimicHabitsCollection; }
            set { _atimicHabitsCollection = value; OnPropertyChanged(nameof(AtimicHabitsCollection)); }
        }
        public Dictionary<int, ObservableCollection<ProgressHistoryModel>> ProgressByHabit
        {
            get { return _progressByHabit; }
            set { _progressByHabit = value; OnPropertyChanged(nameof(ProgressByHabit)); }
        }
        public HabitHistorySelectionComboBoxModel SeledtedHabitHistorySelectionComboBoxoperty
        {
            get { return _seledtedHabitHistorySelectionComboBox; }
            set { _seledtedHabitHistorySelectionComboBox = value; OnPropertyChanged(nameof(SeledtedHabitHistorySelectionComboBoxoperty)); }
        }
        public ObservableCollection<ProgressHistoryModel> ProgressCollection
        {
            get { return _progressCollection; }
            set { _progressCollection = value; OnPropertyChanged(nameof(ProgressCollection)); }
        }

        public DateTime DateToEdit
        {
            get { return _dateToEdit; }
            set { _dateToEdit = value; OnPropertyChanged(nameof(DateToEdit)); }
        }

        public bool IsHabitDoneToEdit
        {
            get { return _isHabitDoneToEdit; }
            set { _isHabitDoneToEdit = value; OnPropertyChanged(nameof(IsHabitDoneToEdit)); }
        }
        #endregion



        private readonly IProgressHistoryService _progressHistoryService;
        private readonly IAtomicHabitService _atomicHabitService;

        public ICommand EditProgresHistoryCommand { get; set; }

        public ProgressHistoryViewModel(IAtomicHabitService atomicHabitService, IProgressHistoryService progressHistoryService)
        {
            ProgressByHabit = new Dictionary<int, ObservableCollection<ProgressHistoryModel>>();

            AtimicHabitsCollection = new ObservableCollection<AtomicHabitModel>();

            _progressHistoryService = progressHistoryService;
            _atomicHabitService = atomicHabitService;

            EditProgresHistoryCommand = new RelayCommandAsync(EditProgresHistory);

            _ = LoadAtimicHabitsAsync();
            _ = LoadProgressByHabitsAsync();


            HabitHistorySelectionComboBox = new ObservableCollection<HabitHistorySelectionComboBoxModel>();
            SeledtedHabitHistorySelectionComboBoxoperty = new HabitHistorySelectionComboBoxModel();

            _ = GenerateHabitHistoryComboBox();

        }

        public void AddProgresHistory(object parameter)
        {
            int i = 1;
        }

        public void DelateProgresHistory(object parameter)
        {
            int i = 1;
        }

        public async Task EditProgresHistory(object parameter)
        {
            await _progressHistoryService.EditProgressAsync(DateToEdit, IsHabitDoneToEdit, SeledtedHabitHistorySelectionComboBoxoperty.Id);
        }

        public async Task GenerateHabitHistoryComboBox()
        {
            await LoadAtimicHabitsAsync();
            await LoadProgressByHabitsAsync();

            HabitHistorySelectionComboBox.Clear();
            foreach (var item in ProgressByHabit)
            {
                HabitHistorySelectionComboBox.Add(new HabitHistorySelectionComboBoxModel
                {
                    Id = item.Key,
                    Content = AtimicHabitsCollection.FirstOrDefault(x => x.Id == item.Key).Title,
                    ProgressCollection = item.Value,
                    SelectedDates = ExtractDateFromHabitHistoryCollection(),
                });
            }
        }

        private ObservableCollection<DateTime> ExtractDateFromHabitHistoryCollection()
        {
            var uniqueDates = new ObservableCollection<DateTime>();

            foreach (var item in ProgressByHabit)
            {
                foreach(var model in item.Value)
                {
                    if (model.IsHabitChecked == true && model.AtomicHabitId == item.Key)
                    {
                        uniqueDates.Add(model.HabitCheckDateTime);
                    }
                }
            }
            return uniqueDates;
        }

        public async Task LoadProgressByHabitsAsync()
        {
            ProgressByHabit.Clear();
            foreach (var atomicHabit in AtimicHabitsCollection)
            {
                IEnumerable<ProgressHistoryModel> progressEnumerable = await _progressHistoryService.GetProgressByHabitIdAsync(atomicHabit.Id);
                var progressCollection = new ObservableCollection<ProgressHistoryModel>(progressEnumerable);
                ProgressByHabit.Add(atomicHabit.Id, progressCollection);
            }
            
        }

        private async Task LoadAtimicHabitsAsync()
        {
            var atomicHabits = await _atomicHabitService.GetAllAsync();
            AtimicHabitsCollection = new ObservableCollection<AtomicHabitModel>(atomicHabits);
        }
    }
}
