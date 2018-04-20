using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEnge.Database;
using IEnge.Database.Entities;
using IEnge.Helpers;
using IEnge.Models;
using IEnge.Service.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IEnge.Controllers.Auth
{
    [Route(Constants.Strings.AuthControllerName)]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtFactory _jwtFactory;
        private readonly DatabaseContext _context;
        private readonly ILogger<JwtFactory> _logger;

        public AuthController(IConfiguration configuration, IJwtFactory jwtFactory, DatabaseContext context,
            ILogger<JwtFactory> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _jwtFactory = jwtFactory;
            _context = context;
        }

        //Post Auth/validate/token
        [HttpPost("validate/token")]
        public async Task<IActionResult> ValidateToken([FromBody] TokenModel model)
        {
            var validationResponse = await _jwtFactory.ValidateToken(model);
            if (validationResponse.IsValidated)
            {
                var claimsIdentity =
                    await Task.FromResult(
                        _jwtFactory.GenerateClaimsIdentity(validationResponse.UserName, validationResponse.Id));

                var userExist = _context.Users.FirstOrDefault(user => user.Username == validationResponse.UserName);

                if (userExist != null)
                {
                    userExist.LastLogin = DateTimeOffset.Now;
                }
                else
                {
                    var newUser = new User
                    {
                        Id = Guid.Parse(validationResponse.Id),
                        Username = validationResponse.UserName,
                        Created = DateTimeOffset.Now,
                        IsDeleted = false,

                    };
                    _context.Users.Add(newUser);
                }
                return Ok(_jwtFactory.GenerateEncodedToken(validationResponse.UserName, claimsIdentity));
            }

            return Unauthorized();
        }
    }
}
