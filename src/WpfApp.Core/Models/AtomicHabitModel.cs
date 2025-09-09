using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models.Shared;

namespace WpfApp.Core.Models;

public class AtomicHabitModel : BaseEntityModel
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private bool _isHabitDone;
    private int _streak;
    private bool _isTextBlockReadOnly = true;
    private bool _isEditHabitModeOn = false;

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(nameof(Title)); }
    }  
    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(nameof(Description)); }
    }

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

    // klucz obcy
    public int CategoryId { get; set; }

    // navigation property
    private CategoryModel _categoryModel;

    public CategoryModel CategoryModel
    {
        get { return _categoryModel; }
        set { _categoryModel = value; OnPropertyChanged(nameof(CategoryModel)); }
    }


    // Kolekcja historii nawyku
    public ICollection<ProgressHistoryModel> ProgressHistories { get; set; } = new List<ProgressHistoryModel>();

    [NotMapped]
    public bool IsTextBlockReadOnly
    {
        get { return _isTextBlockReadOnly; }
        set { _isTextBlockReadOnly = value; OnPropertyChanged(nameof(IsTextBlockReadOnly)); }
    }
    [NotMapped]
    public bool IsEditHabitModeOn
    {
        get { return _isEditHabitModeOn; }
        set { _isEditHabitModeOn = value; OnPropertyChanged(nameof(IsEditHabitModeOn)); }
    }
}
