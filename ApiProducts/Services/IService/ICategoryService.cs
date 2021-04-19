using ApiProducts.Models.DTO.Category;
using ApiProducts.Models.DTO.Category.Request;
using ApiProducts.Models.DTO.SubCategory;
using ApiProducts.Models.DTO.SubCategory.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Services.IService
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAll();
        Task<CategoryDTO> GetById(int categoryId);
        Task<CategoryDTO> Add(CategoryAddRequest model);
        Task<bool> ExistsId(int categoryId);
        Task<bool> ExistsName(string name);

    }
}
