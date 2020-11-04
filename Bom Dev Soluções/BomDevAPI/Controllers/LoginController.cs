using BomDevAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BomDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;

            return "Bem vindo " + userName;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetValues")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", "value3" };
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string userName, string passoword)
        {
            UserModel login = new UserModel()
            {
                UserName = userName,
                Password = passoword
            };

            var user = AutenticarUsuario(login);

            if(user != null)
            {
                var token = GenerateJSONWebToken(user);

                return Ok(new { 
                    token = token
                });
            }

            return Unauthorized("Acesso não autorizado.");
        }

        private string GenerateJSONWebToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAdress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims, null, DateTime.Now.AddMinutes(120), credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodeToken;
        }

        private UserModel AutenticarUsuario(UserModel login)
        {
            UserModel userModel = null;

            if(string.Equals(login.UserName, "guilherme", System.StringComparison.OrdinalIgnoreCase) &&
                string.Equals(login.Password, "@anr2761", System.StringComparison.OrdinalIgnoreCase))
            {
                userModel = new UserModel()
                {
                    UserName = "guilherme",
                    Password = "@anr2761",
                    EmailAdress = "guilherme@bomdev.com.br",
                    Role = "Admin"
                };
            }

            return userModel;
        }
    }
}
