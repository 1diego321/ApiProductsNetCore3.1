using ApiProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Repositories.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();
        Task<Category> GetById(int categoryId);
        Task<bool> Add(Category model);
        Task<bool> ExistsId(int categoryId);
        Task<bool> ExistsName(string name);

    }
}
