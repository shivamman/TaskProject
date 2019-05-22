using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace project.application.Interfaces
{
    public interface IJwtService
    {
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id, IList<string> roles);
        Task<string> GenerateJwt(ClaimsIdentity identity, string userName);
    }
}
