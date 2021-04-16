using ApiProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Repositories.IRepository
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> Authenticate(string credential);
        Task<bool> Register(ApplicationUser model);
        Task<List<ApplicationUser>> GetAll();
        Task<ApplicationUser> GetById(int id);
        Task<bool> DisableOrEnableUser(ApplicationUser model);
        Task<bool> ExistsUserName(string userName);
        Task<bool> ExistsEmail(string email);
        Task<bool> ExistsId(int id);
    }
}
