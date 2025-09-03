using WpfApp.Core.Data;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace WpfApp.Core.Services
{
    public class ProgressHistoryService : IProgressHistoryService
    {
        private readonly AppDbContext _context;

        public ProgressHistoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasProgressTodayAsync(int atomicHabitId)
        {
            var today = DateTime.Now.Date;

            return await _context.ProgressHistory
                                 .AnyAsync(p => p.AtomicHabitId == atomicHabitId &&
                                                p.HabitCheckDateTime.Date == today);
        }

        public async Task<ProgressHistoryModel> AddProgressAsync(AtomicHabitModel atomicHabit)
        {
            if (atomicHabit == null)
                throw new ArgumentNullException(nameof(atomicHabit));

            var progress = new ProgressHistoryModel
            {
                HabitCheckDateTime = DateTime.Now,
                IsHabitChecked = atomicHabit.IsHabitDone,
                AtomicHabitId = atomicHabit.Id,
            };


            var progressHistory = await GetProgressByHabitIdAsync(atomicHabit.Id); 

            var today = DateTime.Now.Date;

            // Sprawdzamy, czy istnieje już wpis z dzisiejszą datą
            var todayProgress = progressHistory.FirstOrDefault(p => p.HabitCheckDateTime.Date == today);

            if (todayProgress != null)
            {
                // Jeżeli już był wpis dziś, to aktualizujemy
                todayProgress.IsHabitChecked = progress.IsHabitChecked;
                todayProgress.HabitCheckDateTime = DateTime.Now;
                _context.ProgressHistory.Update(todayProgress);
            }
            else
            {
                // Dodajemy wpis do DbSet jeżeli puste
                _context.ProgressHistory.Add(progress);
            }
 
            await _context.SaveChangesAsync();

            return progress;
        }

        public async Task<IEnumerable<ProgressHistoryModel>> GetProgressByHabitIdAsync(int atomicHabitId)
        {
            return await _context.ProgressHistory
                                 .Where(p => p.AtomicHabitId == atomicHabitId)
                                 .OrderBy(p => p.HabitCheckDateTime) // opcjonalnie sortowanie po dacie
                                 .ToListAsync();
        }


    }
}
