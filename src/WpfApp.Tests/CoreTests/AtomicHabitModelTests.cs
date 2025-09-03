using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Models;

namespace WpfApp.Tests.CoreTests
{
    public class AtomicHabitModelTests
    {
        [Fact]
        public void OnMidnight_ShouldResetAndIncrementCorrectly()
        {
            var atomicHabitModel = new AtomicHabitModel
            {
                Title = "Test Tit",
                Description = "Test Desc",
                IsHabitDone = true,
                Streak = 1,
            };

            atomicHabitModel.OnMidnight();

            Assert.Equal(2, atomicHabitModel.Streak);    // increment
            Assert.False(atomicHabitModel.IsHabitDone);  // convert to false
        }
    }
}
