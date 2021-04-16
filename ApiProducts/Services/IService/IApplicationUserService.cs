using ApiProducts.Models;
using ApiProducts.Models.DTO.ApplicationUser;
using ApiProducts.Models.DTO.ApplicationUser.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Services.IService
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserLoginDTO> Authenticate(ApplicationUserAuthRequest model);
        Task<bool> Register(ApplicationUserRegisterRequest model);
        Task<List<ApplicationUserDTO>> GetAll();
        Task<ApplicationUserDTO> GetById(int id);
        Task<bool> DisableOrEnableUser(int id);
        Task<bool> ExistsUserName(string userName);
        Task<bool> ExistsEmail(string email);
        Task<bool> ExistsId(int id);
    }
}
