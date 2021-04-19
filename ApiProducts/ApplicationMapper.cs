using ApiProducts.Models;
using ApiProducts.Models.DTO.ApplicationUser;
using ApiProducts.Models.DTO.ApplicationUser.Request;
using ApiProducts.Models.DTO.Category;
using ApiProducts.Models.DTO.Product;
using ApiProducts.Models.DTO.Product.Request;
using ApiProducts.Models.DTO.SubCategory;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            //ApplicationUser
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserRegisterRequest>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserLoginDTO>().ReverseMap();

            //Product
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductAddRequest>().ReverseMap();
            CreateMap<Product, ProductUpdateRequest>().ReverseMap();

            //Category
            CreateMap<Category, CategoryDTO>().ReverseMap();

            //SubCategory
            CreateMap<SubCategory, SubCategoryDTO>().ReverseMap();
        }
    }
}
