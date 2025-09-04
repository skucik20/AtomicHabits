using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;
using WpfApp.Core.Data;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Services;

namespace WpfApp.Tests.CoreTests
{
    public class AtomicHabitServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // separet base for each test
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllHabits()
        {
            // Arrange
            var context = GetDbContext();
            context.AtomicHabits.AddRange(
                new AtomicHabitModel { Id = 1, Title = "Test 1" },
                new AtomicHabitModel { Id = 2, Title = "Test 2" }
            );
            await context.SaveChangesAsync();

            var progressMock = new Mock<IProgressHistoryService>();
            var service = new AtomicHabitService(context, progressMock.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddAsync_AddsHabitToDatabase()
        {
            var context = GetDbContext();
            var progressMock = new Mock<IProgressHistoryService>();
            var service = new AtomicHabitService(context, progressMock.Object);

            var habit = new AtomicHabitModel { 
                Id = 1, 
                Title = "Title 1",
                Description = "Test description",
                IsHabitDone = true,
                Streak = 2,
            };

            await service.AddAsync(habit);

            Assert.Equal(1, context.AtomicHabits.Count());
            Assert.Equal("Title 1", context.AtomicHabits.First().Title);
            Assert.Equal("Test description", context.AtomicHabits.First().Description);
            Assert.True(context.AtomicHabits.First().IsHabitDone);
            Assert.Equal(2, context.AtomicHabits.First().Streak);
            
        }

        [Fact]
        public async Task UpdateAsync_UpdatesHabit()
        {
            var context = GetDbContext();
            var habit = new AtomicHabitModel
            {
                Id = 1,
                Title = "Title 1",
                Description = "Test description",
                IsHabitDone = true,
                Streak = 2,
            };

            context.AtomicHabits.Add(habit);
            await context.SaveChangesAsync();

            var progressMock = new Mock<IProgressHistoryService>();
            var service = new AtomicHabitService(context, progressMock.Object);

            habit.IsHabitDone = false;
            habit.Streak += 1;
            await service.UpdateAsync(habit);

            Assert.False(context.AtomicHabits.First().IsHabitDone);
            Assert.Equal(3, context.AtomicHabits.First().Streak);
        }

        [Fact]
        public async Task DeleteAsync_RemovesHabit()
        {
            var context = GetDbContext();
            var habit = new AtomicHabitModel
            {
                Id = 1,
                Title = "Title 1",
                Description = "Test description",
                IsHabitDone = true,
                Streak = 2,
            };

            context.AtomicHabits.Add(habit);
            await context.SaveChangesAsync();

            var progressMock = new Mock<IProgressHistoryService>();
            var service = new AtomicHabitService(context, progressMock.Object);

            await service.DeleteAsync(1);

            Assert.Empty(context.AtomicHabits);
        }

        [Fact]
        /// <summary>
        /// If the habit was completed yesterday and there is no progress today,
        /// the streak should be incremented by 1 and IsHabitDone set to false.
        /// </summary>
        public async Task HasTodayAtomicHabitChecked_ShouldIncrementStreak_WhenHabitWasDoneYesterday()
        {
            // Arrange
            var context = GetDbContext();
            var habit = new AtomicHabitModel
            {
                Id = 1,
                Title = "Title",
                Description = "Test habit is done",
                IsHabitDone = true,
                Streak = 5,
            };

            context.AtomicHabits.Add(habit);
            
            
            await context.SaveChangesAsync();

            var progressMock = new Mock<IProgressHistoryService>();
            progressMock.Setup(p => p.HasProgressTodayAsync(1)).ReturnsAsync(false);

            var service = new AtomicHabitService(context, progressMock.Object);

            // Act
            await service.HasTodayAtomicHabitChecked();

            // Assert
            var updatedHabit = context.AtomicHabits.First();

            Assert.False(updatedHabit.IsHabitDone);
            Assert.Equal(6, updatedHabit.Streak); // habit was done so streak increment

        }

        [Fact]
        /// <summary>
        /// If the habit was not completed yesterday and there is no progress today,
        /// the streak should be reset to 0 and IsHabitDone remains false.
        /// </summary>
        public async Task HasTodayAtomicHabitChecked_ShouldResetStreak_WhenHabitWasNotDoneYesterday()
        {
            // Arrange
            var context = GetDbContext();
            var habit = new AtomicHabitModel
            {
                Id = 1,
                Title = "Title",
                Description = "Test habit is not done",
                IsHabitDone = false,
                Streak = 4,
            };
            context.AtomicHabits.Add(habit);
            await context.SaveChangesAsync();

            var progressMock = new Mock<IProgressHistoryService>();
            progressMock.Setup(p => p.HasProgressTodayAsync(2)).ReturnsAsync(false);

            var service = new AtomicHabitService(context, progressMock.Object);

            // Act
            await service.HasTodayAtomicHabitChecked();

            // Assert
            var updatedHabit = context.AtomicHabits.First();
            Assert.False(updatedHabit.IsHabitDone);
            Assert.Equal(0, updatedHabit.Streak); // habit was done so streak reset
        }

        [Fact]
        /// <summary>
        /// If there is progress today, the streak should not change
        /// and IsHabitDone remains it was (true).
        /// </summary>
        public async Task HasTodayAtomicHabitChecked_DoesNotResetIfProgressToday()
        {
            var context = GetDbContext();
            var habit = new AtomicHabitModel
            {
                Id = 1,
                Title = "Title",
                Description = "Test habit is done",
                IsHabitDone = true,
                Streak = 4,
            }; 
            context.AtomicHabits.Add(habit);
            await context.SaveChangesAsync();

            var progressMock = new Mock<IProgressHistoryService>();
            progressMock.Setup(p => p.HasProgressTodayAsync(1)).ReturnsAsync(true);

            var service = new AtomicHabitService(context, progressMock.Object);

            await service.HasTodayAtomicHabitChecked();

            var updatedHabit = context.AtomicHabits.First();

            Assert.True(updatedHabit.IsHabitDone);
            Assert.Equal(4, updatedHabit.Streak);
        }

        [Fact]
        /// <summary>
        /// If there is progress today, the streak should not change
        /// and IsHabitDone remains as it was (false).
        /// </summary>
        public async Task HasTodayAtomicHabitChecked_DoesNotResetIfNotProgressToday()
        {
            var context = GetDbContext();
            var habit = new AtomicHabitModel
            {
                Id = 1,
                Title = "Title",
                Description = "Test habit is not done",
                IsHabitDone = false,
                Streak = 4,
            }; 
            context.AtomicHabits.Add(habit);
            await context.SaveChangesAsync();

            var progressMock = new Mock<IProgressHistoryService>();
            progressMock.Setup(p => p.HasProgressTodayAsync(1)).ReturnsAsync(true);

            var service = new AtomicHabitService(context, progressMock.Object);

            await service.HasTodayAtomicHabitChecked();

            var updatedHabit = context.AtomicHabits.First();

            Assert.False(updatedHabit.IsHabitDone);
            Assert.Equal(4, updatedHabit.Streak);
        }

        [Fact]
        public async Task ResetValues_ResetsAllHabits()
        {
            var context = GetDbContext();
            context.AtomicHabits.AddRange(
                new AtomicHabitModel {
                    Id = 1,
                    Title = "Title 1",
                    Description = "Test habit is done",
                    IsHabitDone = true,
                    Streak = 4,
                },
                new AtomicHabitModel {
                    Id = 2,
                    Title = "Title 2",
                    Description = "Test habit is not done",
                    IsHabitDone = false,
                    Streak = 3,
                }
            );
            await context.SaveChangesAsync();

            var progressMock = new Mock<IProgressHistoryService>();
            var service = new AtomicHabitService(context, progressMock.Object);

            await service.ResetValues();

            var habits = context.AtomicHabits.ToList();
            Assert.All(habits, h => Assert.False(h.IsHabitDone));
            Assert.Equal(0, habits.First(h => h.Id == 2).Streak); // bo było false zatem reset
            Assert.Equal(5, habits.First(h => h.Id == 1).Streak); // bo było true i inkrementacja
        }


    }
}
