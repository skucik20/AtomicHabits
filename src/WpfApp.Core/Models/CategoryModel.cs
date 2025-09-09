using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfApp.Core.Models.Shared;

namespace WpfApp.Core.Models
{
    public class CategoryModel : BaseEntityModel
    {
		private string _categoryName = string.Empty;
		private string _categoryColor = string.Empty;

		public string CategoryName
		{
			get { return _categoryName; }
			set { _categoryName = value; }
		}

		public string CategoryColor
        {
			get { return _categoryColor; }
			set { _categoryColor = value; OnPropertyChanged(nameof(CategoryColor)); }
		}

        // relacja odwrotna (opcjonalna)
        public ICollection<AtomicHabitModel> AtomicHabits { get; set; }

    }
}
