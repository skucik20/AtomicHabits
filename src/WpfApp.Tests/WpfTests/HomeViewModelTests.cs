using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;
using WpfApp.Core.Services;
using WpfApp.Wpf.Helpers;
using WpfApp.Wpf.ViewModels;

namespace WpfApp.Tests.WpfTests
{
    public class HomeViewModelTests
    {
        private readonly Mock<IAtomicHabitService> _habitServiceMock;
        private readonly Mock<IProgressHistoryService> _progressServiceMock;


        public HomeViewModelTests()
        {
            _habitServiceMock = new Mock<IAtomicHabitService>();
            _progressServiceMock = new Mock<IProgressHistoryService>();
        }

        [Fact]
        public async Task Constructor_LoadsData()
        {
            // Arrange
            var habits = new List<AtomicHabitModel>
            {
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
            };


            _habitServiceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(habits);


            // Act
            var vm = new HomeViewModel(_habitServiceMock.Object, _progressServiceMock.Object);


            // Wait for async LoadData() to complete
            await Task.Delay(50);


            // Assert
            _habitServiceMock.Verify(s => s.GetAllAsync(), Times.Once);
            Assert.Equal(2, vm.AtimicHabitsCollection.Count); Assert.Collection(vm.AtimicHabitsCollection,
            item =>
            {
                Assert.Equal(1, item.Id);
                Assert.Equal("Title 1", item.Title);
                Assert.Equal("Test habit is done", item.Description);
                Assert.True(item.IsHabitDone);
                Assert.Equal(4, item.Streak);
            },
            item =>
            {
                Assert.Equal(2, item.Id);
                Assert.Equal("Title 2", item.Title);
                Assert.Equal("Test habit is not done", item.Description);
                Assert.False(item.IsHabitDone);
                Assert.Equal(3, item.Streak);
            });
        }

        [Fact]
        public async Task ChangingHabitDone_UpdatesServices()
        {
            // Arrange
            var habit = new AtomicHabitModel {
                Id = 1,
                Title = "Title 1",
                Description = "Test habit is done",
                IsHabitDone = false,
                Streak = 4,
            };

            _habitServiceMock.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<AtomicHabitModel> { habit });


            var vm = new HomeViewModel(_habitServiceMock.Object, _progressServiceMock.Object);
            await Task.Delay(50);


            // Act – zmiana property
            habit.IsHabitDone = true;


            await Task.Delay(50); // daj czas na async handler


            // Assert
            _progressServiceMock.Verify(p => p.AddProgressAsync(habit), Times.Once);
            _habitServiceMock.Verify(p => p.UpdateAsync(habit), Times.Once);
        }

    }
}
