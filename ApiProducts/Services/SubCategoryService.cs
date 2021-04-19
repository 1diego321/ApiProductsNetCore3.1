using ApiProducts.Models;
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
    public class SubCategoryService : ISubCategoryService
    {
        #region DEPENDENCIES DECLARATION
        private readonly ISubCategoryRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region CONSTRUCTOR
        public SubCategoryService(ISubCategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region METHODS
        public async Task<SubCategoryDTO> Add(SubCategoryAddRequest model)
        {
            var oSubCategory = _mapper.Map<SubCategory>(model);

            if (await _repository.Add(oSubCategory))
            {
                return _mapper.Map<SubCategoryDTO>(oSubCategory);
            }

            return null;
        }

        public async Task<bool> ExistsId(int subCategoryId)
        {
            return await _repository.ExistsId(subCategoryId);
        }

        public async Task<bool> ExistsName(int categoryId, string name)
        {
            return await _repository.ExistsName(categoryId, name);
        }

        public async Task<List<SubCategoryDTO>> GetAll()
        {
            return (await _repository.GetAll()).Select(s => _mapper.Map<SubCategoryDTO>(s)).ToList();
        }

        public async Task<List<SubCategoryDTO>> GetAllByCategoryId(int categoryId)
        {
            return (await _repository.GetAllByCategoryId(categoryId)).Select(s => _mapper.Map<SubCategoryDTO>(s)).ToList();
        }

        public async Task<SubCategoryDTO> GetById(int subCategoryId)
        {
            return _mapper.Map<SubCategoryDTO>(await _repository.GetSubCategoryById(subCategoryId));
        }
        #endregion
    }
}
