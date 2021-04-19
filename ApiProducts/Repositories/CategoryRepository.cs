using ApiProducts.Data;
using ApiProducts.Models;
using ApiProducts.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        #region OBJECT DECLARATION
        private readonly ApplicationDbContext _context;
        #endregion

        #region CONSTRUCTOR
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region METHODS
        public async Task<List<Category>> GetAll()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category> GetById(int categoryId)
        {
            return await _context.Category.FindAsync(categoryId);
        }

        public async Task<bool> Add(Category model)
        {
            _context.Category.Add(model);

            return await Save();
        }

        public async Task<bool> ExistsId(int categoryId)
        {
            return await _context.Category.AnyAsync(c => c.Id == categoryId);
        }

        public async Task<bool> ExistsName(string name)
        {
            return await _context.Category.AnyAsync(c =>
                c.Name.Trim().ToLower() == name.Trim().ToLower());
        }
        #endregion

        #region UTILITY METHODS
        private async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
