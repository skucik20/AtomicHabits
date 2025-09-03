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
        private readonly AppDbContext _context;

        public AtomicHabitService(AppDbContext context)
        {
            _context = context;
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
    }
}
