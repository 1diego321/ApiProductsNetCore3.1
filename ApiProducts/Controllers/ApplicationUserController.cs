using ApiProducts.Common;
using ApiProducts.Models.DTO.Application;
using ApiProducts.Models.DTO.ApplicationUser.Request;
using ApiProducts.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Controllers
{
    [ApiExplorerSettings(GroupName = nameof(ApplicationUserController))]
    [Route("api/User")]
    [ApiController]
    [Authorize]
    public class ApplicationUserController : ControllerBase
    {
        #region DEPENDENCY DECLARATIONS
        private readonly IApplicationUserService _service;
        #endregion

        #region CONSTRUCTOR
        public ApplicationUserController(IApplicationUserService service)
        {
            _service = service;
        }
        #endregion

        #region ACTION METHODS
        /// <summary>
        /// User register 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        public async Task<IActionResult> Register(ApplicationUserRegisterRequest model)
        {
            Response oR = new Response();

            try
            {
                if (!ModelState.IsValid)
                {
                    oR.Status = StatusCodes.Status400BadRequest;
                    oR.Message = Messages.ValidationsFailed;
                    oR.Data = GetModelErrors(ModelState);

                    return BadRequest(oR);
                }

                if (await _service.ExistsUserName(model.UserName))
                {
                    oR.Status = StatusCodes.Status400BadRequest;
                    oR.Message = "El nombre de usuario ya está en uso.";

                    return BadRequest(oR);
                }

                if (await _service.ExistsEmail(model.Email))
                {
                    oR.Status = StatusCodes.Status400BadRequest;
                    oR.Message = "El correo electronico ya está en uso.";

                    return BadRequest(oR);
                }

                if (!await _service.Register(model))
                {
                    oR.Status = StatusCodes.Status500InternalServerError;
                    oR.Message = Messages.InternalServerError;

                    return StatusCode(StatusCodes.Status500InternalServerError, oR);
                }

                oR.Status = StatusCodes.Status200OK;               
                
                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = Messages.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        /// <summary>
        /// Gets a List of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        public async Task<IActionResult> GetAll()
        {
            Response oR = new Response();

            try
            {
                oR.Data = await _service.GetAll();
                oR.Status = StatusCodes.Status200OK;

                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = Messages.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        /// <summary>
        /// Gets a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Response oR = new Response();

            try
            {
                var oUser = await _service.GetById(id);

                if(oUser == null)
                {
                    oR.Status = StatusCodes.Status404NotFound;
                    oR.Message = Messages.ResourceNotFound;

                    return NotFound(oR);
                }

                oR.Data = oUser;
                oR.Status = StatusCodes.Status200OK;

                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = Messages.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        /// <summary>
        /// Disables or enables user accounts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DisableOrEnableUser(int id)
        {
            Response oR = new Response();

            try
            {
                if(await _service.ExistsId(id))
                {
                    oR.Status = StatusCodes.Status404NotFound;
                    oR.Message = Messages.ResourceNotFound;

                    return NotFound(oR);
                }

                if(!await _service.DisableOrEnableUser(id))
                {
                    oR.Status = StatusCodes.Status500InternalServerError;
                    oR.Message = Messages.InternalServerError;

                    return StatusCode(StatusCodes.Status500InternalServerError, oR);
                }

                oR.Status = StatusCodes.Status204NoContent;

                return NoContent();
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
        private List<string> GetModelErrors(ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)).ToList();
        }
        #endregion
    }
}
