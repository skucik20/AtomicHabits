using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Models.Shared;

namespace WpfApp.Core.Models
{
    public class ProgressHistoryModel : BaseEntityModel
    {
        public DateTime HabitCheckDateTime { get; set; }
        public bool IsHabitChecked { get; set; }

        // Klucz obcy wskazujący na AtomicHabitModel
        public int AtomicHabitId { get; set; }

        // Właściwość nawigacyjna do powiązanego AtomicHabitModel
        public AtomicHabitModel AtomicHabit { get; set; }
    }
}
