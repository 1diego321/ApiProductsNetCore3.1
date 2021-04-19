using ApiProducts.Models.DTO.Product;
using ApiProducts.Models.DTO.Product.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Services.IService
{
    public interface IProductService
    {
        Task<ProductDTO> Add(ProductAddRequest model);
        Task<bool> Update(ProductUpdateRequest model);
        Task<bool> DisableOrEnable(int id);
        Task<List<ProductDTO>> GetAll();
        Task<ProductDTO> GetById(int id);
        Task<bool> ExistsId(int id);
    }
}
