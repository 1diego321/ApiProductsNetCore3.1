using ApiProducts.Models;
using ApiProducts.Models.DTO.Category;
using ApiProducts.Models.DTO.Category.Request;
using ApiProducts.Models.DTO.SubCategory;
using ApiProducts.Models.DTO.SubCategory.Request;
using ApiProducts.Repositories.IRepository;
using ApiProducts.Services.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Services
{
    public class CategoryService : ICategoryService
    {
        #region DEPENDENCIES DECLARATION
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region CONSTRUCTOR
        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region METHODS
        public async Task<CategoryDTO> Add(CategoryAddRequest model)
        {
            var oCategory = _mapper.Map<Category>(model);

            if (await _repository.Add(oCategory))
            {
                return _mapper.Map<CategoryDTO>(oCategory);
            }

            return null;
        }

        public async Task<List<CategoryDTO>> GetAll()
        {
            return (await _repository.GetAll()).Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }

        public async Task<CategoryDTO> GetById(int categoryId)
        {
            return _mapper.Map<CategoryDTO>(await _repository.GetById(categoryId));
        }

        public async Task<bool> ExistsId(int categoryId)
        {
            return await _repository.ExistsId(categoryId);
        }

        public async Task<bool> ExistsName(string name)
        {
            return await _repository.ExistsName(name);
        }
        #endregion
    }
}
