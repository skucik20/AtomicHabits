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
        public DailyResetService(AppDbContext context, IAtomicHabitService atomicHabitService)
        {
            _context = context;
            _atomicHabitService = atomicHabitService;
            _timer = new System.Timers.Timer();
            // TODO zmienić na sprawdzanie co minutę
            _timer.Interval = 1000; //* 60; // sprawdzanie co minutę
            _timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            _timer.Start();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if (now.Hour == 0 && now.Minute == 0)
            {
                await ResetValues();
            }
        }

        private async Task ResetValues()
        {
            
            var atomicHabits = await _atomicHabitService.GetAllAsync();

            foreach (var habit in atomicHabits)
            {
                StreakSupport(habit);
                habit.IsHabitDone = false; // must be after StreakFlag

                //_context.AtomicHabits.Update(habit);
            }
            await _context.SaveChangesAsync();
        }

        private void StreakSupport(AtomicHabitModel habit)
        {
            switch (habit.IsHabitDone)
            {
                case false:
                    habit.Streak = 0;
                    break;
                case true:
                    habit.Streak++;
                    break;
            }
        }
    }
}
