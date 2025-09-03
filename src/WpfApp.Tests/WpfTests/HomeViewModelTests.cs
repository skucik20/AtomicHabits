using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Models;
using WpfApp.Core.Services;
using WpfApp.Wpf.Helpers;

namespace WpfApp.Tests.WpfTests
{
    public class HomeViewModelTests
    {
        //[Fact]
        //public void OnMidnightReset_ShouldUpdatePeopleCollection()
        //{
        //    // Arrange
        //    var atomicHabitModel = new List<AtomicHabitModel> { new AtomicHabitModel { Title = "Test title" } };
        //    var atomicHabitServiceMock = new Mock<AtomicHabitService>(null);
        //    atomicHabitServiceMock.Setup(s => s.GetAllAsync()).Returns(atomicHabitModel);

        //    var schedulerMock = new Mock<MidnightResetScheduler>(null);
        //    var vm = new HomeViewModel(atomicHabitServiceMock.Object, schedulerMock.Object);

        //    // Act: symulacja eventu
        //    schedulerMock.Raise(s => s.OnMidnightReset += null, System.EventArgs.Empty);

        //    // Assert: kolekcja w VM odświeżona
        //    Assert.Single(vm.AtomicHabitModel);
        //    Assert.Equal(25, vm.AtomicHabitModel.First().Age);
        //    Assert.Equal(10, vm.AtomicHabitModel.First().Points);
        //}



    }
}
