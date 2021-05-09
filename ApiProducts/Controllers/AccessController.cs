using ApiProducts.Common;
using ApiProducts.Models.DTO.Application;
using ApiProducts.Models.DTO.ApplicationUser;
using ApiProducts.Models.DTO.ApplicationUser.Request;
using ApiProducts.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiProducts.Controllers
{
    [ApiExplorerSettings(GroupName = nameof(AccessController))]
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        #region DEPENDENCY DECLARATIONS
        private readonly IApplicationUserService _service;
        private readonly IConfiguration _configuration;
        #endregion

        #region CONSTRUCTOR
        public AccessController(IApplicationUserService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }
        #endregion

        #region ACTION METHODS
        /// <summary>
        /// Used for user authentication request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        public async Task<IActionResult> Authenticate(ApplicationUserAuthRequest model)
        {
            Response oR = new Response();

            try
            {
                var oUser = await _service.Authenticate(model);

                if(oUser == null)
                {
                    oR.Status = StatusCodes.Status400BadRequest;
                    oR.Message = Messages.InvalidUserNamePassword;

                    return BadRequest(oR);
                }

                if (oUser.ApplicationUserStatusId == 2)
                {
                    oR.Status = StatusCodes.Status400BadRequest;
                    oR.Message = Messages.AccountDisabled;

                    return BadRequest(oR);
                }

                SetToken(oUser);

                oR.Status = StatusCodes.Status200OK;
                oR.Data = oUser;

                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = Messages.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }
        #endregion

        #region UTILITY METHODS
        /// <summary>
        /// Sets JWT Token
        /// </summary>
        /// <param name="model"></param>
        private void SetToken(ApplicationUserLoginDTO model)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                new Claim(ClaimTypes.Name, model.FullName.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(0.5),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            model.Token = tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Returns a List<string> of model errors
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private List<string> GetModelErrors(ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)).ToList();
        }
        #endregion
    }
}
