using ApiProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Repositories.IRepository
{
    public interface IProductRepository
    {
        Task<bool> Add(Product model);
        Task<bool> Update(Product model);
        Task<bool> DisableOrEnable(Product model);
        Task<List<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<bool> ExistsId(int id);
    }
}
