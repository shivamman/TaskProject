using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using project.application.Interfaces;
using project.domain.DTO;
using project.domain.Models;
using project.persistence.Context;
using project.persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace project.application.Services
{
    public class AccountService : IAccountService
    {
        private readonly CASContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _uow;
        private readonly IJwtService _jwtService;
        private readonly JwtIssuerOptions _jwtOptions;

        public AccountService(UserManager<AppUser> userManager, IUnitOfWork uow, IJwtService jwtService, IOptions<JwtIssuerOptions> jwtOptions, CASContext context)
        {
            _context = context;
            _uow = uow;
            _userManager = userManager;
            _jwtService = jwtService;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }


        public async Task<IdentityResult> CreateAsync(AppUser userIdentity, string password)
        {
            try
            {
                
                var result = await _userManager.CreateAsync(userIdentity, password);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IdentityResult> DeleteAsync(AppUser userIdentity)
        {
            try
            {
                var result = await _userManager.DeleteAsync(userIdentity);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IdentityResult> AddToRoleAsync(AppUser userIdentity, string roleName)
        {
            try
            {
                var result = await _userManager.AddToRoleAsync(userIdentity, roleName);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<AppUser> FindByNameAsync(string userName) =>
            await _userManager.FindByNameAsync(userName);

        public async Task<IList<string>> GetUserRoles(AppUser appUser) =>
            await _userManager.GetRolesAsync(appUser);

        public async Task<bool> CheckPasswordAsync(AppUser user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        public async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                    return await Task.FromResult<ClaimsIdentity>(null);

                // get the user to verifty
                var userToVerify = await FindByNameAsync(userName);

                if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

                //var userClaims = await GetClaimsAsync(userToVerify);
                IList<string> userRolesList = await GetUserRoles(userToVerify);
                // check the credentials
                if (await CheckPasswordAsync(userToVerify, password))
                {
                    return await Task.FromResult(_jwtService.GenerateClaimsIdentity(userName, userToVerify.Id.ToString(), userRolesList));
                }

                // Credentials are invalid, or account doesn't exist
                return await Task.FromResult<ClaimsIdentity>(null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IdentityResult> AddClaimAsync(AppUser userIdentity, string roleName) =>
            await _userManager.AddClaimAsync(userIdentity, new Claim(ClaimTypes.Role, roleName));

        public async Task RemoveClaimsAndRolesAsync(AppUser userIdentity)
        {
            try
            {
                var claims = await _userManager.GetClaimsAsync(userIdentity);
                var roles = await _userManager.GetRolesAsync(userIdentity);
                if (claims.Count != 0)
                {
                    await _userManager.RemoveClaimsAsync(userIdentity, claims);
                }
                if (roles.Count != 0)
                {
                    await _userManager.RemoveFromRolesAsync(userIdentity, roles);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<string> ConfirmReset(string token, string userId, string newPassword)
        {
            if (token != null && userId != null)
            {
                var userFromDatabase = await _userManager.FindByIdAsync(userId);
                //var result = await _userManager.ConfirmEmailAsync(userFromDatabase, token);
                var result = await _userManager.ResetPasswordAsync(userFromDatabase, token, newPassword);
                if (result.Succeeded)
                {
                    return "Change Success";
                }
                else
                {
                    var r = (result.Errors).FirstOrDefault().Description;
                    return r;
                }
            }
            return "Invalid Token";
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null) throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }

    }
}
