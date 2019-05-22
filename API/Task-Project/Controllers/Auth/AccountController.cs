using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using project.application.Interfaces;
using project.domain.DTO;
using System;
using System.Threading.Tasks;
using project.application.StaticServices;
using project.application.Services;

namespace Task_Design.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, IJwtService jwtService)
        {
            _accountService = accountService;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> PostAsync([FromBody]AppUserDTO regDTO)
        {
            try
            {
                var appUser = new AppUser()
                {
                    Email = regDTO.Email,
                    FirstName = regDTO.FirstName,
                    LastName = regDTO.LastName,
                    UserName = regDTO.UserName
                };
                IdentityResult result = await _accountService.CreateAsync(appUser, regDTO.Password);
                if (result.Succeeded)
                {
                    await _accountService.AddToRoleAsync(appUser, Constant.Operation.Admin);
                    return Ok();
                }
                else return BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginDTO loginModel)
        {
            try
            {
                var claimsIdentity = await _accountService.GetClaimsIdentity(loginModel.UserName, loginModel.Password);
                if (claimsIdentity == null)
                {
                    return Unauthorized();
                }
                string jwt = await _jwtService.GenerateJwt(claimsIdentity, loginModel.UserName);
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResult("Login failed. " + ex.Message));
            }
        }
    }
}