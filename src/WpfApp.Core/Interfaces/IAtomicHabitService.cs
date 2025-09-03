using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Models;

namespace WpfApp.Core.Interfaces
{
    public interface IAtomicHabitService
    {
        Task<List<AtomicHabitModel>> GetAllAsync();
        Task AddAsync(AtomicHabitModel atomicHabit);
        Task UpdateAsync(AtomicHabitModel atomicHabit);
        Task DeleteAsync(int id);
    }
}
