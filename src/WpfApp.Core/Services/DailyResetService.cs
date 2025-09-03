using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Timers;
using WpfApp.Core.Data;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;

namespace WpfApp.Core.Services
{
    public class DailyResetService : IDailyResetService
    {
        private readonly AppDbContext _context;
        private readonly System.Timers.Timer _timer;
        private readonly IAtomicHabitService _atomicHabitService;
        public bool IsTimerEnabled => _timer.Enabled;

        public DailyResetService(AppDbContext context, IAtomicHabitService atomicHabitService)
        {
            _context = context;
            _atomicHabitService = atomicHabitService;
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000 * 60; // sprawdzanie co minutę
            _timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            _timer.Start();
        }

        //private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    var now = DateTime.Now;
        //    if (now.Hour == 0 && now.Minute == 0)
        //    {
        //        await _atomicHabitService.ResetValues();
        //    }
        //}

        protected virtual async Task OnTimerElapsed(DateTime now)
        {
            if (now.Hour == 0 && now.Minute == 0)
            {
                await _atomicHabitService.ResetValues();
            }
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await OnTimerElapsed(DateTime.Now);
        }




    }
}
