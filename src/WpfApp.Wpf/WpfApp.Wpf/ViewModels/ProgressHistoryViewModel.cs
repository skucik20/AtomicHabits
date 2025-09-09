using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Models.Shared;
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
        private HabitHistorySelectionComboBoxModel _seledtedHabitHistorySelectionComboBox;
        private ObservableCollection<ProgressHistoryModel> _progressCollection;
        public ObservableCollection<HabitHistorySelectionComboBoxModel> HabitHistorySelectionComboBox { get; set; }

        private ObservableCollection<AtomicHabitModel> _atimicHabitsCollection;
        public ObservableCollection<AtomicHabitModel> AtimicHabitsCollection
        {
            get { return _atimicHabitsCollection; }
            set { _atimicHabitsCollection = value; OnPropertyChanged(nameof(AtimicHabitsCollection)); }
        }

        private Dictionary<int, ObservableCollection<ProgressHistoryModel>> _progressByHabit;
        public Dictionary<int, ObservableCollection<ProgressHistoryModel>> ProgressByHabit
        {
            get { return _progressByHabit; }
            set { _progressByHabit = value; OnPropertyChanged(nameof(ProgressByHabit)); }
        }


        public HabitHistorySelectionComboBoxModel SeledtedHabitHistorySelectionComboBoxoperty
        {
            get { return _seledtedHabitHistorySelectionComboBox; }
            set { _seledtedHabitHistorySelectionComboBox = value; OnPropertyChanged(nameof(SeledtedHabitHistorySelectionComboBoxoperty));
            }
        }

        public ObservableCollection<ProgressHistoryModel> ProgressCollection
        {
            get { return _progressCollection; }
            set { _progressCollection = value; OnPropertyChanged(nameof(ProgressCollection)); }
        }

        private readonly IProgressHistoryService _progressHistoryService;
        private readonly IAtomicHabitService _atomicHabitService;
        public ProgressHistoryViewModel(IAtomicHabitService atomicHabitService, IProgressHistoryService progressHistoryService)
        {
            ProgressByHabit = new Dictionary<int, ObservableCollection<ProgressHistoryModel>>();

            AtimicHabitsCollection = new ObservableCollection<AtomicHabitModel>();

            _progressHistoryService = progressHistoryService;
            _atomicHabitService = atomicHabitService;

            _ = LoadAtimicHabitsAsync();
            _ = LoadProgressByHabitsAsync();


            HabitHistorySelectionComboBox = new ObservableCollection<HabitHistorySelectionComboBoxModel>();
            SeledtedHabitHistorySelectionComboBoxoperty = new HabitHistorySelectionComboBoxModel();

            _ = GenerateHabitHistoryComboBox();

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
