using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prject.domain.Models;
using project.application.Interfaces;

namespace Task_Project.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ProductController : BaseController
    {
        private IProductsService _productsService;
        public ProductController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("test")]
        public ActionResult Test() {
            return Ok();
        }

        [HttpGet("list")]
        public ActionResult GetProductsList() {
            try
            {
                return Ok(_productsService.GetProductsList());
            }
            catch (Exception ex)
            {               
                throw ex;
            }            
        }

        [HttpPost("create")]
        public ActionResult CreateProduct([FromBody]Product products)
        {
            try
            {
                _productsService.CreateProduct(products);
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("getById/{id}")]
        public ActionResult GetProductById(Guid Id)
        {
            try
            {
                var product = _productsService.GetProductsById(Id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("delete/{id}")]
        public ActionResult DeleteProduct(Guid id)
        {
            try
            {
                _productsService.DeleteProduct(id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("update")]
        public ActionResult UpdateProduct([FromBody]Product product)
        {
            try
            {
                _productsService.UpdateProduct(product);
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}