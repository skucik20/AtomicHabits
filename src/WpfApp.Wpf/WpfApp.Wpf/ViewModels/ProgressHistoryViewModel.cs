using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    }
    public class ProgressHistoryViewModel :BaseViewModel
    {
        private HabitHistorySelectionComboBoxModel _seledtedHabitHistorySelectionComboBox;
        private ObservableCollection<ProgressHistoryModel> _progressCollection;
        public ObservableCollection<HabitHistorySelectionComboBoxModel> HabitHistorySelectionComboBox { get; set; }
        public ObservableCollection<AtomicHabitModel> AtimicHabitsCollection { get; set; }
        
        public Dictionary<int, ObservableCollection<ProgressHistoryModel>> ProgressByHabit { get; set; }


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
            foreach(var item in ProgressByHabit)
            {
                HabitHistorySelectionComboBox.Add(new HabitHistorySelectionComboBoxModel
                {
                    Id = item.Key,
                    Content = AtimicHabitsCollection.FirstOrDefault(x => x.Id == item.Key).Title,
                    ProgressCollection = item.Value
                });
            }

        }

        public async Task LoadProgressByHabitsAsync()
        {

            foreach(var atomicHabit in AtimicHabitsCollection)
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
