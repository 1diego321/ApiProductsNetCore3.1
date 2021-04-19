﻿using ApiProducts.Common;
using ApiProducts.Models.DTO.Application;
using ApiProducts.Models.DTO.Product.Request;
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
    [ApiExplorerSettings(GroupName = nameof(ProductController))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        #region DEPENDENCIES DECLARATIONS
        private readonly IProductService _service;
        #endregion

        #region CONSTRUCTOR
        public ProductController(IProductService service)
        {
            _service = service;
        }
        #endregion

        #region ACTION METHODS
        [HttpGet]
        [AllowAnonymous]
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

        [HttpGet("{id:int}", Name = nameof(GetById))]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            Response oR = new Response();
            
            try
            {
                var oProduct = await _service.GetById(id);

                if(oProduct != null)
                {
                    if(oProduct.ProductStatusId == 1)
                    {
                        oR.Data = oProduct;
                        oR.Status = StatusCodes.Status200OK;

                        return Ok(oR);
                    }
                    else
                    {
                        oR.Status = StatusCodes.Status400BadRequest;
                        oR.Message = Messages.ResourceDisabled;

                        return BadRequest(oR);
                    }
                }
                else
                {
                    oR.Status = StatusCodes.Status404NotFound;
                    oR.Message = Messages.ResourceNotFound;

                    return NotFound(oR);
                }
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = Messages.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductAddRequest model)
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

                var oProduct = await _service.Add(model);

                if (oProduct != null)
                {
                    oR.Status = StatusCodes.Status201Created;
                    oR.Data = oProduct;

                    return CreatedAtAction(nameof(GetById), new { id = oProduct.Id }, oR);
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

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateRequest model)
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

                if(!await _service.ExistsId(model.Id))
                {
                    oR.Message = Messages.ResourceNotFound;
                    oR.Status = StatusCodes.Status404NotFound;

                    return NotFound(oR);
                }

                if(!await _service.Update(model))
                {
                    oR.Status = StatusCodes.Status500InternalServerError;
                    oR.Message = Messages.InternalServerError; 

                    return StatusCode(StatusCodes.Status500InternalServerError, oR);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                oR.Status = StatusCodes.Status500InternalServerError;
                oR.Message = Messages.InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, oR);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DisableOrEnable(int id)
        {
            Response oR = new Response();

            try
            {
                if(!await _service.ExistsId(id))
                {
                    oR.Status = StatusCodes.Status404NotFound;
                    oR.Message = Messages.ResourceNotFound;

                    return NotFound(oR);
                }

                if(!await _service.DisableOrEnable(id))
                {
                    oR.Status = StatusCodes.Status500InternalServerError;
                    oR.Message = Messages.InternalServerError;

                    return StatusCode(StatusCodes.Status500InternalServerError, oR);
                }

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
