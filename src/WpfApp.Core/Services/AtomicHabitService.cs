using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Data;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;

namespace WpfApp.Core.Services
{
    public class AtomicHabitService : IAtomicHabitService
    {
        private readonly IProgressHistoryService _progressHistoryService;
        private readonly AppDbContext _context;

        public AtomicHabitService(AppDbContext context, IProgressHistoryService progressHistoryService)
        {
            _context = context;
            _progressHistoryService = progressHistoryService;
        }

        public async Task<List<AtomicHabitModel>> GetAllAsync()
        {
            return await _context.AtomicHabits.ToListAsync();
        }

        public async Task AddAsync(AtomicHabitModel atomicHabit)
        {
            _context.AtomicHabits.Add(atomicHabit);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(AtomicHabitModel atomicHabit)
        {
            _context.AtomicHabits.Update(atomicHabit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var atomicHabit = await _context.AtomicHabits.FindAsync(id);
            if (atomicHabit != null)
            {
                _context.AtomicHabits.Remove(atomicHabit);
                await _context.SaveChangesAsync();
            }
        }

        public async Task HasTodayAtomicHabitChecked()
        {
            var atomicHabits = await GetAllAsync();
            foreach (var atomicHabit in atomicHabits)
            {
                var HasProgressToday = await _progressHistoryService.HasProgressTodayAsync(atomicHabit.Id);
                if (!HasProgressToday)
                {
                    await ResetValue(atomicHabit);
                }
            }
        }

        private async Task ResetValue(AtomicHabitModel atomicHabit)
        {

            StreakSupport(atomicHabit);
            atomicHabit.IsHabitDone = false; // must be after StreakFlag

            await _context.SaveChangesAsync();
        }

        public async Task ResetValues()
        {

            var atomicHabits = await GetAllAsync();

            foreach (var habit in atomicHabits)
            {
                await ResetValue(habit);
            }
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
