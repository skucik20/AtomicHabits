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
    public class ProgressHistoryServiceTests
    {
        private AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }


        [Fact]
        public async Task HasProgressTodayAsync_ShouldReturnFalse_WhenNoProgress()
        {
            // Arrange
            var context = GetDbContext(nameof(HasProgressTodayAsync_ShouldReturnFalse_WhenNoProgress));
            var service = new ProgressHistoryService(context);

            // Act
            var result = await service.HasProgressTodayAsync(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task HasProgressTodayAsync_ShouldReturnTrue_WhenProgressExistsToday()
        {
            // Arrange
            var context = GetDbContext(nameof(HasProgressTodayAsync_ShouldReturnTrue_WhenProgressExistsToday));
            context.ProgressHistory.Add(new ProgressHistoryModel
            {
                AtomicHabitId = 1,
                HabitCheckDateTime = DateTime.Now,
                IsHabitChecked = true
            });
            await context.SaveChangesAsync();

            var service = new ProgressHistoryService(context);

            // Act
            var result = await service.HasProgressTodayAsync(1);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task AddProgressAsync_ShouldThrow_WhenAtomicHabitIsNull()
        {
            // Arrange
            var context = GetDbContext(nameof(AddProgressAsync_ShouldThrow_WhenAtomicHabitIsNull));
            var service = new ProgressHistoryService(context);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddProgressAsync(null));
        }


        [Fact]
        public async Task AddProgressAsync_ShouldAddNewProgress_WhenNoProgressToday()
        {
            // Arrange
            var context = GetDbContext(nameof(AddProgressAsync_ShouldAddNewProgress_WhenNoProgressToday));
            var service = new ProgressHistoryService(context);

            var habit = new AtomicHabitModel
            {
                Id = 1,
                Title = "Title 1",
                Description = "Test description",
                IsHabitDone = true,
                Streak = 2,
            };

            // Act
            var result = await service.AddProgressAsync(habit);

            // Assert
            var progress = await context.ProgressHistory.FirstOrDefaultAsync();
            Assert.NotNull(progress);
            Assert.Equal(habit.Id, progress.AtomicHabitId);
            Assert.True(progress.IsHabitChecked);
        }


        [Fact]
        public async Task AddProgressAsync_ShouldUpdateProgress_WhenProgressExistsToday()
        {
            // Arrange
            var context = GetDbContext(nameof(AddProgressAsync_ShouldUpdateProgress_WhenProgressExistsToday));
            var existingProgress = new ProgressHistoryModel
            {
                AtomicHabitId = 1,
                HabitCheckDateTime = DateTime.Now.Date.AddHours(8),
                IsHabitChecked = false
            };
            context.ProgressHistory.Add(existingProgress);
            await context.SaveChangesAsync();

            var service = new ProgressHistoryService(context);

            var habit = new AtomicHabitModel
            {
                Id = 1,
                Title = "Title 1",
                Description = "Test description",
                IsHabitDone = true,
                Streak = 2,
            };

            // Act
            var result = await service.AddProgressAsync(habit);

            // Assert
            var progress = await context.ProgressHistory.FirstOrDefaultAsync();
            Assert.NotNull(progress);
            Assert.Equal(habit.Id, progress.AtomicHabitId);
            Assert.True(progress.IsHabitChecked); // zostało zaktualizowane
            Assert.True(progress.HabitCheckDateTime.Date == DateTime.Now.Date);
        }


        [Fact]
        public async Task GetProgressByHabitIdAsync_ShouldReturnOrderedProgress()
        {
            // Arrange
            var context = GetDbContext(nameof(GetProgressByHabitIdAsync_ShouldReturnOrderedProgress));
            context.ProgressHistory.AddRange(new List<ProgressHistoryModel>
            {
                new ProgressHistoryModel { AtomicHabitId = 1, HabitCheckDateTime = DateTime.Now.AddDays(-1), IsHabitChecked = true },
                new ProgressHistoryModel { AtomicHabitId = 1, HabitCheckDateTime = DateTime.Now.AddDays(-2), IsHabitChecked = false },
            });
            await context.SaveChangesAsync();

            var service = new ProgressHistoryService(context);

            // Act
            var result = await service.GetProgressByHabitIdAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.First().HabitCheckDateTime < result.Last().HabitCheckDateTime); // sortowanie rosnąco
        }

    }
}
