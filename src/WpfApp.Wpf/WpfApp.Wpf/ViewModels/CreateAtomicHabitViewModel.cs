using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Services;
using WpfApp.Wpf.Helpers.Commands;
using WpfApp.Wpf.ViewModels.Shared;

namespace WpfApp.Wpf.ViewModels
{
    public class CreateAtomicHabitViewModel : BaseViewModel
    {
        private readonly IAtomicHabitService _atomicHabitService;

        private string _habitTitle = string.Empty;
        private string _habitDescription = string.Empty;
        public ICommand AddHabitCommand { get; set; }
        
		public string HabitTitle
		{
			get { return _habitTitle; }
			set { _habitTitle = value; }
		}

		public  string HabitDescription
        {
			get { return _habitDescription; }
			set { _habitDescription = value; }
		}

        public CreateAtomicHabitViewModel(IAtomicHabitService atomicHabitService)
        {
            _atomicHabitService = atomicHabitService;
            AddHabitCommand = new RelayCommandAsync(AddHabit);
        }

        public async Task AddHabit(object parametr)
        {
            await _atomicHabitService.AddAsync(new AtomicHabitModel { Title = HabitDescription, Description = HabitDescription });
        }

    }
}
