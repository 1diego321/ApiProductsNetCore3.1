using ApiProducts.Common;
using ApiProducts.Models.DTO.Application;
using ApiProducts.Models.DTO.Category.Request;
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
    [ApiExplorerSettings(GroupName = nameof(CategoryController))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        #region DEPENDENCIES DECLARATION
        private readonly ICategoryService _service;
        #endregion

        #region CONSTRUCTOR
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        #endregion

        #region ACTION METHODS
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromQuery(Name = "id")] int id)
        {
            Response oR = new Response();

            try
            {
                var oCategory = await _service.GetById(id);

                if (oCategory == null)
                {
                    oR.Status = StatusCodes.Status404NotFound;
                    oR.Message = Messages.ResourceNotFound;

                    return NotFound(oR);
                }

                oR.Status = StatusCodes.Status200OK;
                oR.Data = oCategory;

                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = Messages.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            Response oR = new Response();

            try
            {
                var lst = await _service.GetAll();

                oR.Status = StatusCodes.Status200OK;
                oR.Data = lst;

                return Ok(oR);
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = Messages.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddRequest model)
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

                if (!await _service.ExistsName(model.Name))
                {
                    ModelState.AddModelError("Name", Messages.ResourceNameAlreadyExists);

                    oR.Status = StatusCodes.Status400BadRequest;
                    oR.Message = Messages.ValidationsFailed;
                    oR.Data = GetModelErrors(ModelState);

                    return BadRequest(oR);
                }

                var oCategory = await _service.Add(model);

                if (oCategory != null)
                {
                    oR.Status = StatusCodes.Status201Created;
                    oR.Data = oCategory;

                    return CreatedAtAction(nameof(GetById), new { id = oCategory.Id }, oR);
                }
                else
                {
                    oR.Status = StatusCodes.Status500InternalServerError;
                    oR.Message = Messages.InternalServerError;

                    return StatusCode(StatusCodes.Status500InternalServerError, oR);
                }
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
