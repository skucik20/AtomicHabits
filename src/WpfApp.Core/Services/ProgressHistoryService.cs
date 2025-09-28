using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using WpfApp.Core.Data;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;


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
        /// <summary>
        /// Checks if the user is not entering data from the future.
        /// If the date is valid:
        ///   - updates the progressHistory for the given date, or
        ///   - creates a new entry if one does not exist.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task EditProgressAsync(DateTime entryDate,bool isHabitDoneToEdit, int id)
        {
            DateTime today = DateTime.Today;

            if (entryDate >= today)
            {
                throw new ArgumentException("Future dates are not allowed.");
            }
            var progressList = await GetProgressByHabitIdAsync(id);

            bool exists = progressList.Any(h => h.HabitCheckDateTime.Date == entryDate.Date);
            if(exists == true)
            {
                var existingProgress = progressList.Where(x => x.HabitCheckDateTime.Date == entryDate.Date).ToList();
                existingProgress[0].IsHabitChecked = isHabitDoneToEdit;
                _context.ProgressHistory.Update(existingProgress[0]);
            }
            else
            {
                var newProgress = new ProgressHistoryModel
                {
                    Id = 19,
                    HabitCheckDateTime = entryDate,
                    IsHabitChecked = isHabitDoneToEdit,
                    AtomicHabitId = id
                };
                _context.ProgressHistory.Add(newProgress);
             }


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
