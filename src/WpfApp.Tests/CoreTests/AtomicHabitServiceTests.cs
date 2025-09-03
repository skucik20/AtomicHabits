using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Data;
using WpfApp.Core.Models;
using WpfApp.Core.Services;

namespace WpfApp.Tests.CoreTests
{
    public class AtomicHabitServiceTests
    {
        /// <summary>
        /// I’m testing the AddAsync method in AtomicHabitService using an in-memory database.
        /// </summary>
        [Fact]
        public async Task AddAsync_ShouldAddAtomicHabit()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using var context = new AppDbContext(options);
            var service = new AtomicHabitService(context);

            await service.AddAsync(new AtomicHabitModel { Id = 1, Title = "Test title", Description = "Test desc" });

            var atomicHabit = await service.GetAllAsync();
            Assert.Single(atomicHabit);
            Assert.Equal("Test title", atomicHabit[0].Title);
            Assert.Equal("Test desc", atomicHabit[0].Description);

            await service.DeleteAsync(1);
        }
    }
}
