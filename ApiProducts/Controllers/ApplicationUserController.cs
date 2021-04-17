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
    [ApiExplorerSettings(GroupName = "ApplicationUserController")]
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
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(ApplicationUserRegisterRequest model)
        {
            Response oR = new Response();

            try
            {
                if (!ModelState.IsValid)
                {
                    oR.Status = 400;
                    oR.Message = "Se han incumplido una o más validaciones.";
                    oR.Data = GetModelErrors(ModelState);

                    return BadRequest(oR);
                }

                if (await _service.ExistsUserName(model.UserName))
                {
                    oR.Status = 400;
                    oR.Message = "El nombre de usuario ya está en uso.";

                    return BadRequest(oR);
                }

                if (await _service.ExistsEmail(model.Email))
                {
                    oR.Status = 400;
                    oR.Message = "El correo electronico ya está en uso.";

                    return BadRequest(oR);
                }

                if (!await _service.Register(model))
                {
                    oR.Status = StatusCodes.Status500InternalServerError;
                    oR.Message = "Ha ocurrido un error interno en el servidor.";

                    return StatusCode(StatusCodes.Status500InternalServerError, oR);
                }

                oR.Status = 200;               
                
                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = "Ha ocurrido un error interno en el servidor.";

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Response oR = new Response();

            try
            {
                oR.Data = await _service.GetAll();
                oR.Status = 200;

                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = "Ha ocurrido un error interno en el servidor.";

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

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
                    oR.Message = "No se encontró el recurso solicitado.";

                    return NotFound(oR);
                }

                oR.Data = oUser;
                oR.Status = 200;

                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = "Ha ocurrido un error interno en el servidor.";

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> DisableOrEnableUser(int id)
        {
            Response oR = new Response();

            try
            {
                if(await _service.ExistsId(id))
                {
                    oR.Status = StatusCodes.Status404NotFound;
                    oR.Message = "No se encontró el recurso solicitado.";

                    return NotFound(oR);
                }

                if(!await _service.DisableOrEnableUser(id))
                {
                    oR.Status = StatusCodes.Status500InternalServerError;
                    oR.Message = "Ha ocurrido un error interno en el servidor.";

                    return StatusCode(StatusCodes.Status500InternalServerError, oR);
                }

                oR.Status = StatusCodes.Status200OK;

                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = "Ha ocurrido un error interno en el servidor.";

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
