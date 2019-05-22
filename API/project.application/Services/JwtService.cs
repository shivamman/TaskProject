using Microsoft.Extensions.Options;
using project.application.Interfaces;
using project.domain.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Principal;
using project.application.Helpers;
using Newtonsoft.Json;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace project.application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtService(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }
        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id, IList<string> roles)
        {
            try
            {
                List<Claim> claimList = new List<Claim>();
                claimList.Add(new Claim(Constants.Strings.JwtClaimIdentifiers.Id, id));
                foreach (var item in roles)
                {
                    claimList.Add(new Claim(ClaimTypes.Role, item));
                }
                return new ClaimsIdentity(new GenericIdentity(userName, "Token"), claimList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> GenerateJwt(ClaimsIdentity identity, string userName)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                var response = new
                {
                    username = userName,
                    id = identity.Claims.Single(c => c.Type == "id").Value,
                    role = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList(),
                    auth_token = await GenerateEncodedToken(userName, identity),
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
                };
                return JsonConvert.SerializeObject(response, serializerSettings);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            try
            {
                var claimList = new List<Claim>() {
                     new Claim(JwtRegisteredClaimNames.Sub, userName),
                     new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                     new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                     identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Id)
                };
                var claimRoles = identity.FindAll(ClaimTypes.Role);
                foreach (var item in claimRoles)
                {
                    claimList.Add(item);
                }

                // Create the JWT security token and encode it.
                var jwt = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claimList,
                    notBefore: _jwtOptions.NotBefore,
                    expires: _jwtOptions.Expiration,
                    signingCredentials: _jwtOptions.SigningCredentials);

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return encodedJwt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

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
