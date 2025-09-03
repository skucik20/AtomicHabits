using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Models;

namespace WpfApp.Core.Interfaces
{
    public interface IProgressHistoryService
    {
        Task<ProgressHistoryModel> AddProgressAsync(AtomicHabitModel atomicHabit);
        Task<IEnumerable<ProgressHistoryModel>> GetProgressByHabitIdAsync(int atomicHabitId);
        Task<bool> HasProgressTodayAsync(int atomicHabitId);
    }
}
