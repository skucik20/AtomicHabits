using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Wpf.ViewModels.Shared;

namespace WpfApp.Wpf.ViewModels
{
    public class EditHabitViewModel : BaseViewModel
    {
        private readonly IAtomicHabitService _atomicHabitService;
        public AtomicHabitModel _atomicHabit { get; set; }
        public ICommand SaveCommand { get; }
        public EditHabitViewModel(IAtomicHabitService atomicHabitService, AtomicHabitModel atomicHabit)
        {
            _atomicHabitService = atomicHabitService;
            _atomicHabit = atomicHabit;
        }
        private async void Save()
        {
            await _atomicHabitService.UpdateAsync(_atomicHabit);
        }
    }
}
