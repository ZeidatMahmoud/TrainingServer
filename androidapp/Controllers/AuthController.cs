using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using androidapp.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace androidapp.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly DataBaseContext _context;
        public AuthController(DataBaseContext context)
        {
            _context = context;
        }
        //api/auth/auth2
        [HttpPost("auth2")]
        public dynamic token([FromForm] string username, [FromForm]string password)
        {
            bool found = _context.Login.Any(info =>
            info.Name == username
            && info.Password == password
         );
            if (found == true)
            {

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name,username)
                   };

                var key = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes("YOUR-PRIVATE-RSA-KEY"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "yourdomain.com",
                    audience: "yourdomain.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);
                var handler = new JwtSecurityTokenHandler();


                String x = ((handler).WriteToken(token)).Split("")[0];
                var b = new { authenticated = true, access_token = x, token_type = "Bearer", expires_in = 1 };


                return b;
            }

            return BadRequest("un authorized");
        }

    }

}
    
