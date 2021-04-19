using ApiProducts.Models.DTO.SubCategory;
using ApiProducts.Models.DTO.SubCategory.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Services.IService
{
    public interface ISubCategoryService
    {
        Task<List<SubCategoryDTO>> GetAll();
        Task<List<SubCategoryDTO>> GetAllByCategoryId(int categoryId);
        Task<SubCategoryDTO> GetById(int subCategoryId);
        Task<SubCategoryDTO> Add(SubCategoryAddRequest model);
        Task<bool> ExistsId(int subCategoryId);
        Task<bool> ExistsName(int categoryId, string name);
    }
}
