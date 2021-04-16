using ApiProducts.Data;
using ApiProducts.Models;
using ApiProducts.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        #region OBJECT DECLARATION
        private readonly ApplicationDbContext _context;
        #endregion

        #region CONSTRUCTOR
        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region METHODS
        public async Task<ApplicationUser> Authenticate(string credential)
        {
            return await _context.ApplicationUser.FirstOrDefaultAsync(u => u.UserName == credential || u.Email == credential);
        }

        public async Task<bool> Register(ApplicationUser model)
        {
            await _context.ApplicationUser.AddAsync(model);

            return await Save();
        }

        public async Task<List<ApplicationUser>> GetAll()
        {
            return await _context.ApplicationUser.Include(x => x.ApplicationUserStatus).ToListAsync();
        }

        public async Task<ApplicationUser> GetById(int id)
        {
            return await _context.ApplicationUser.Include(x => x.ApplicationUserStatus).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> DisableOrEnableUser(ApplicationUser model)
        {
            _context.Entry(model).State = EntityState.Modified;

            return await Save();
        }

        public async Task<bool> ExistsUserName(string userName)
        {
            return await _context.ApplicationUser.AnyAsync(u => u.UserName.ToLower().Trim() == userName.ToLower().Trim());
        }

        public async Task<bool> ExistsEmail(string email)
        {
            return await _context.ApplicationUser.AnyAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
        }

        public async Task<bool> ExistsId(int id)
        {
            return await _context.ApplicationUser.AnyAsync(u => u.Id == id);
        }
        #endregion

        #region UTILITY METHODS
        private async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
