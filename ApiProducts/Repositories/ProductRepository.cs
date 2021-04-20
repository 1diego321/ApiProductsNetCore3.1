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
    public class ProductRepository : IProductRepository
    {
        #region OBJECT DECLARATION
        private readonly ApplicationDbContext _context;
        #endregion

        #region CONSTRUCTOR
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region METHODS
        public async Task<bool> Add(Product model)
        {
            await _context.Product.AddAsync(model);

            return await Save();
        }

        public async Task<bool> Update(Product model)
        {
            _context.Entry(model).State = EntityState.Modified;

            return await Save();
        }

        public async Task<bool> DisableOrEnable(Product model)
        {
            _context.Entry(model).State = EntityState.Modified;

            return await Save();
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Product.FindAsync(id);
        }

        public async Task<bool> ExistsId(int id)
        {
            return await _context.Product.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsCode(string code)
        {
            return await _context.Product.AnyAsync(p => p.Code.Trim().ToLower() == code.Trim().ToLower());
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
