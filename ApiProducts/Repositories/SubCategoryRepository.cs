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
    public class SubCategoryRepository : ISubCategoryRepository
    {
        #region OBJECT DECLARATION
        private readonly ApplicationDbContext _context;
        #endregion

        #region CONSTRUCTOR
        public SubCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region METHODS
        public async Task<bool> Add(SubCategory model)
        {
            _context.SubCategory.Add(model);

            return await Save();
        }

        public async Task<List<SubCategory>> GetAll()
        {
            return await _context.SubCategory.ToListAsync();
        }

        public async Task<List<SubCategory>> GetAllByCategoryId(int categoryId)
        {
            return await _context.SubCategory.Where(s => s.CategoryId == categoryId).ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryById(int subCategoryId)
        {
            return await _context.SubCategory.FindAsync(subCategoryId);
        }

        public async Task<bool> ExistsId(int subCategoryId)
        {
            return await _context.SubCategory.AnyAsync(s => s.Id == subCategoryId);
        }

        public async Task<bool> ExistsName(int categoryId, string name)
        {
            return await _context.SubCategory.AnyAsync(s =>
                s.CategoryId == categoryId && s.Name.Trim().ToLower() == name.Trim().ToLower());
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
