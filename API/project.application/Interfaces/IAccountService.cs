using Microsoft.AspNetCore.Identity;
using project.domain.DTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace project.application.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> AddClaimAsync(AppUser userIdentity, string roleName);
        Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password);
        Task<IList<string>> GetUserRoles(AppUser appUser);
        Task<IdentityResult> CreateAsync(AppUser userIdentity, string password);
        Task<IdentityResult> DeleteAsync(AppUser userIdentity);
        Task<IdentityResult> AddToRoleAsync(AppUser userIdentity, string roleName);
        Task RemoveClaimsAndRolesAsync(AppUser userIdentity);
    }
}
