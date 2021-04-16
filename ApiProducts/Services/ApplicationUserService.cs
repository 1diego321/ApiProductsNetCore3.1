using ApiProducts.Models;
using ApiProducts.Models.DTO.ApplicationUser;
using ApiProducts.Models.DTO.ApplicationUser.Request;
using ApiProducts.Repositories.IRepository;
using ApiProducts.Services.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region ABSTRACTION DECLARATIONS
        private readonly IApplicationUserRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region CONSTRUCTOR
        public ApplicationUserService(IApplicationUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region METHODS
        public async Task<ApplicationUserLoginDTO> Authenticate(ApplicationUserAuthRequest model)
        {
            var oUser = await _repository.Authenticate(model.UserNameOrEmail);

            if (oUser == null) return null;

            if (!VerifyPasswordHash(model.Password, oUser.PasswordHash, oUser.PasswordSalt)) return null;

            return _mapper.Map<ApplicationUserLoginDTO>(oUser);
        }

        public async Task<bool> Register(ApplicationUserRegisterRequest model)
        {
            byte[] passwordHash, passwordSalt;
            var oUser = _mapper.Map<ApplicationUser>(model);

            CreatePasswordHashAndSalt(model.Password,out passwordHash, out passwordSalt);

            oUser.ApplicationUserStatusId = 1;
            oUser.CreatedDate = DateTime.Now;
            oUser.PasswordHash = passwordHash;
            oUser.PasswordSalt = passwordSalt;

            return await _repository.Register(oUser);
        }

        public async Task<List<ApplicationUserDTO>> GetAll()
        {
            var lst = await _repository.GetAll();

            return lst.Select(u => _mapper.Map<ApplicationUserDTO>(u)).ToList();
        }

        public async Task<ApplicationUserDTO> GetById(int id)
        {
            return _mapper.Map<ApplicationUserDTO>(await _repository.GetById(id));
        }

        public async Task<bool> DisableOrEnableUser(int id)
        {
            var oUser = await _repository.GetById(id);

            oUser.ApplicationUserStatusId = oUser.ApplicationUserStatusId == 1 ? 2 : 1;

            return await _repository.DisableOrEnableUser(oUser);
        }

        public async Task<bool> ExistsUserName(string userName)
        {
            return await _repository.ExistsUserName(userName);
        }

        public async Task<bool> ExistsEmail(string email)
        {
            return await _repository.ExistsEmail(email);
        }

        public async Task<bool> ExistsId(int id)
        {
            return await _repository.ExistsId(id);
        }
        #endregion

        #region UTILITY METHODS
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }

        private void CreatePasswordHashAndSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion
    }
}
