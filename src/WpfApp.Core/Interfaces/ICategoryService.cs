using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Models;

namespace WpfApp.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetAllAsync();
        Task AddAsync(CategoryModel categoryModel);
        void AddDefaultCatedories();
    }
}
