using ApiProducts.Models;
using ApiProducts.Models.DTO.Product;
using ApiProducts.Models.DTO.Product.Request;
using ApiProducts.Repositories.IRepository;
using ApiProducts.Services.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Services
{
    public class ProductService : IProductService
    {
        #region DEPENDENCIES DECLARATIONS
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region CONSTRUCTOR
        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region METHODS
        public async Task<ProductDTO> Add(ProductAddRequest model)
        {
            var oProduct = _mapper.Map<Product>(model);

            if (model.Image != null)
            {
                string mainRoute = @"C:\ApiProducts\Files\Images";
                string imgName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                string finalRoute = Path.Combine(mainRoute, imgName);

                Directory.CreateDirectory(mainRoute);

                using (var fs = new FileStream(finalRoute, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fs);

                    oProduct.ImageRoute = finalRoute;
                }
            }

            oProduct.ProductStatusId = 1;

            var ok = await _repository.Add(oProduct);

            if (ok)
            {
                return _mapper.Map<ProductDTO>(oProduct);
            }

            return null;
        }

        public async Task<bool> Update(ProductUpdateRequest model)
        {
            var oProduct = await _repository.GetById(model.Id);

            oProduct.Name = model.Name;
            oProduct.Code = model.Code;
            oProduct.Description = model.Description;
            oProduct.Price = model.Price;
            oProduct.Stock = model.Stock;
            oProduct.SubCategoryId = model.SubCategoryId;

            return await _repository.Update(oProduct);
        }

        public async Task<bool> DisableOrEnable(int id)
        {
            var oProduct = await _repository.GetById(id);

            if (oProduct == null) return false;

            oProduct.ProductStatusId = oProduct.ProductStatusId == 1 ? 2 : 1;

            return await _repository.DisableOrEnable(oProduct);
        }

        public async Task<List<ProductDTO>> GetAll()
        {
            return (await _repository.GetAll()).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
        }

        public async Task<ProductDTO> GetById(int id)
        {
            return _mapper.Map<ProductDTO>(await _repository.GetById(id));
        }

        public async Task<bool> ExistsId(int id)
        {
            return await _repository.ExistsId(id);
        }

        public async Task<bool> ExistsCode(string code)
        {
            return await _repository.ExistsCode(code);
        }
        #endregion
    }
}
