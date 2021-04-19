using ApiProducts.Common;
using ApiProducts.Models.DTO.Application;
using ApiProducts.Models.DTO.Category.Request;
using ApiProducts.Models.DTO.SubCategory.Request;
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
    [ApiExplorerSettings(GroupName = nameof(SubCategoryController))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubCategoryController : ControllerBase
    {
        #region DEPENDENCIES DECLARATION
        private readonly ISubCategoryService _service;
        #endregion

        #region CONSTRUCTOR
        public SubCategoryController(ISubCategoryService service)
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
                var oSubCategory = await _service.GetById(id);

                if (oSubCategory == null)
                {
                    oR.Status = StatusCodes.Status404NotFound;
                    oR.Message = Messages.ResourceNotFound;

                    return NotFound(oR);
                }

                oR.Status = StatusCodes.Status200OK;
                oR.Data = oSubCategory;

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

        [HttpGet("{categoryId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByCategoryId(int categoryId)
        {
            Response oR = new Response();

            try
            {
                var lst = await _service.GetAllByCategoryId(categoryId);

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
        public async Task<IActionResult> AddCategory(SubCategoryAddRequest model)
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

                if (!await _service.ExistsName(model.CategoryId, model.Name))
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
