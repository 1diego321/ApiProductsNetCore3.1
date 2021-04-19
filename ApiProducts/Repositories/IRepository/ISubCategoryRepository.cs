using ApiProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Repositories.IRepository
{
    public interface ISubCategoryRepository
    {
        Task<List<SubCategory>> GetAll();
        Task<List<SubCategory>> GetAllByCategoryId(int categoryId);
        Task<SubCategory> GetSubCategoryById(int subCategoryId);
        Task<bool> Add(SubCategory model);
        Task<bool> ExistsId(int subCategoryId);
        Task<bool> ExistsName(int categoryId, string name);
    }
}
