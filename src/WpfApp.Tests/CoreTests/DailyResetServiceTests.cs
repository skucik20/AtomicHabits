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
        private readonly Mock<IAtomicHabitService> _habitServiceMock;

        public DailyResetServiceTests()
        {
            _habitServiceMock = new Mock<IAtomicHabitService>();
        }
        /// <summary>
        /// Sprawdza, czy metoda ResetValues() jest wywoływana dokładnie raz,
        /// gdy zegar wskaże północ (00:00).
        /// </summary>
        [Fact]
        public async Task OnTimerElapsed_AtMidnight_CallsResetValues()
        {
            // Arrange
            var service = new TestableDailyResetService(_habitServiceMock.Object);
            var midnight = new DateTime(2024, 1, 1, 0, 0, 0);

            // Act
            await service.TestOnTimerElapsed(midnight);

            // Assert
            _habitServiceMock.Verify(s => s.ResetValues(), Times.Once);
        }

        /// <summary>
        /// Sprawdza, że metoda ResetValues() NIE jest wywoływana,
        /// jeśli aktualny czas nie jest północą.
        /// </summary>
        [Fact]
        public async Task OnTimerElapsed_NotAtMidnight_DoesNotCallResetValues()
        {
            // Arrange
            var service = new TestableDailyResetService(_habitServiceMock.Object);
            var notMidnight = new DateTime(2024, 1, 1, 10, 30, 0);

            // Act
            await service.TestOnTimerElapsed(notMidnight);

            // Assert
            _habitServiceMock.Verify(s => s.ResetValues(), Times.Never);
        }

        /// <summary>
        /// Sprawdza, że wywołanie Start() włącza wewnętrzny timer,
        /// dzięki czemu serwis rozpoczyna okresowe sprawdzanie czasu.
        /// </summary>
        [Fact]
        public void Start_ShouldEnableTimer()
        {
            // Arrange
            var service = new DailyResetService(null, _habitServiceMock.Object);

            // Act
            service.Start();

            // Assert
            Assert.True(service.IsTimerEnabled);
        }

        /// <summary>
        /// Klasa testowa rozszerzająca DailyResetService,
        /// która pozwala wywoływać logikę OnTimerElapsed ręcznie
        /// z podaną datą i godziną, co upraszcza testowanie.
        /// </summary>
        private class TestableDailyResetService : DailyResetService
        {
            private readonly IAtomicHabitService _atomicHabitService;

            public TestableDailyResetService(IAtomicHabitService habitService)
                : base(null, habitService)
            {
                _atomicHabitService = habitService;
            }

            public Task TestOnTimerElapsed(DateTime now)
            {
                return OnTimerElapsed(now);
            }
        }
    }
}
