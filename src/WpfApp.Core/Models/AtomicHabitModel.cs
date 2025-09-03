using System.ComponentModel;
using System.Windows.Controls;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models.Shared;

namespace WpfApp.Core.Models;

public class AtomicHabitModel : BaseEntityModel
{
    private bool _isHabitDone;
    private int _streak;



    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsHabitDone
    {
        get => _isHabitDone;
        set { _isHabitDone = value; OnPropertyChanged(nameof(IsHabitDone)); }
    }
    public int Streak
    {
        get => _streak;
        set { _streak = value; OnPropertyChanged(nameof(Streak)); }
    }
}
