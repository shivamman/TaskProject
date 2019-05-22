using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.application.Interfaces;
using project.domain.Models;

namespace Task_Project.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ServiceController : BaseController
    {
        private IServicesService _servicesService;

        public ServiceController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet("test")]
        public ActionResult Test()
        {
            return Ok();
        }

        [HttpGet("list")]
        public ActionResult GetServicesList()
        {
            try
            {
                return Ok(_servicesService.GetServicesList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("create")]
        public ActionResult CreateService([FromBody]Service data)
        {
            try
            {
                _servicesService.CreateService(data);
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("getById/{id}")]
        public ActionResult GetServiceById(Guid Id)
        {
            try
            {
                return Ok(_servicesService.GetServicesById(Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("delete/{id}")]
        public ActionResult DeleteService(Guid id) {
            try
            {
                _servicesService.DeleteService(id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("update")]
        public ActionResult UpdateServiceDetails([FromBody] Service data) {
            _servicesService.UpdateService(data);
            return Ok(true);
        }
    }
}