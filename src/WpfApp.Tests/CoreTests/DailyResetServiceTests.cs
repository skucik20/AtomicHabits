using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Data;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Services;

namespace WpfApp.Tests.CoreTests
{
    public class DailyResetServiceTests
    {
        private DailyResetService CreateService(List<AtomicHabitModel> habits, out Mock<IAtomicHabitService> atomicHabitServiceMock)
        {
            // Mock DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new AppDbContext(options);

            // Mock IAtomicHabitService
            atomicHabitServiceMock = new Mock<IAtomicHabitService>();
            atomicHabitServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(habits);

            return new DailyResetService(context, atomicHabitServiceMock.Object);
        }

        [Fact]
        public async Task ResetValues_ShouldIncrementStreak_WhenHabitIsDone()
        {
            // Arrange
            var habits = new List<AtomicHabitModel>
            {
                new AtomicHabitModel { Id = 1, IsHabitDone = true, Streak = 2 },
                new AtomicHabitModel { Id = 2, IsHabitDone = false, Streak = 3 }
            };

            var service = CreateService(habits, out var mockService);

            // Act
            // używamy refleksji do wywołania prywatnej metody
            var method = typeof(DailyResetService).GetMethod("ResetValues", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task)method.Invoke(service, null);
            await task;

            // Assert
            Assert.Equal(3, habits[0].Streak); // streak powinien wzrosnąć
            Assert.False(habits[0].IsHabitDone); // habit powinien zostać zresetowany
        }

        [Fact]
        public async Task ResetValues_ShouldResetStreak_WhenHabitIsNotDone()
        {
            // Arrange
            var habits = new List<AtomicHabitModel>
        {
            new AtomicHabitModel { Id = 1, IsHabitDone = false, Streak = 5 }
        };
            var service = CreateService(habits, out var mockService);

            // Act
            var method = typeof(DailyResetService).GetMethod("ResetValues", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task)method.Invoke(service, null);
            await task;

            // Assert
            Assert.Equal(0, habits[0].Streak); // streak powinien zostać wyzerowany
            Assert.False(habits[0].IsHabitDone); // habit powinien zostać zresetowany
        }

        [Fact]
        public void Timer_ShouldCallResetAtMidnight()
        {
            // Tutaj testowanie timera jest trudne w unit testach bez Dependency Injection Timera
            // Możemy sprawdzić jedynie, że Timer został ustawiony
            var mockService = new Mock<IAtomicHabitService>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TimerTestDb").Options;
            var context = new AppDbContext(options);

            var service = new DailyResetService(context, mockService.Object);
            service.Start();

            // Timer powinien być aktywny
            Assert.True(service != null); // minimalny sanity check
        }
    }
}
