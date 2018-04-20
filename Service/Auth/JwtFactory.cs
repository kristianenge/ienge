using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IEnge.Helpers;
using IEnge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace IEnge.Service.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger<JwtFactory> _logger;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions, ILogger<JwtFactory> logger)
        {
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Rol),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Id)
             };

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task<ValidationResponse> ValidateToken(TokenModel model)
        {
            var validationResponse = new ValidationResponse { IsValidated = false };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.microsoft.com");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", model.Token);

                var response = await client.GetAsync("/v1.0/me");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Token validated successfully!");

                    JToken token = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    var userPrincipalName = token.SelectToken("userPrincipalName").ToString();
                    var mail = token.SelectToken("mail").ToString();
                    var id = token.SelectToken("id").ToString();

                    validationResponse.IsValidated = true;
                    validationResponse.Mail = mail;
                    validationResponse.UserName = userPrincipalName;
                    validationResponse.Id = id;
                    return validationResponse;
                }
                _logger.LogWarning($"Token was NOT validated succcessfully. Response statuscode[{response.StatusCode}] content[{response?.Content?.ReadAsStringAsync()?.Result}]");

                return validationResponse;
            }
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Constants.Strings.JwtClaimIdentifiers.Id, id),
                new Claim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess)
            });
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

    }
}
