using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfApp.Core.Data;
using WpfApp.Core.Interfaces;
using WpfApp.Core.Models;

namespace WpfApp.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task AddAsync(CategoryModel categoryModel)
        {
            _context.Categories.Add(categoryModel);
            await _context.SaveChangesAsync();
        }

        public void AddDefaultCatedories()
        {
            var context = GetAllAsync();

            if(context.Result.Count == 0)
            {

                List<CategoryModel> categoriesList = new List<CategoryModel>
                {
                    new CategoryModel
                    {
                        CategoryName = "Mind (Growth & Learning)",
                        CategoryColor = "#388E3C"
                    },
                    new CategoryModel
                    {
                        CategoryName = "Relationships (Connection & Community)",
                        CategoryColor = "#F57C00"
                    },
                    new CategoryModel
                    {
                        CategoryName = "Spirit/Emotions (Inner Balance)",
                        CategoryColor = "#D32F2F"
                    },
                    new CategoryModel
                    {
                        CategoryName = "Body (Physical Health)",
                        CategoryColor = "#7E57C2"
                    }
                };

                foreach (var category in categoriesList)
                {
                    _ = AddAsync(category);
                }
            }
        }
    }
}
