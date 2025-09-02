using WpfApp.Core.Models.Shared;

namespace WpfApp.Core.Models;

public class AtomicHabitModel : BaseEntityModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
