using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using androidapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using JWT;
using JWT.Serializers;
using JWT.Algorithms;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace androidapp.Controllers
{
    
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DataBaseContext _context;
        //constructor
        public LoginController(IConfiguration configuration,DataBaseContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //api/login/register
        [AllowAnonymous]
        [HttpPost("register")]
        public Login register([FromBody]Login request )
        {
            _context.Login.Add(request);
            _context.SaveChanges();
            return request;

        }
       //api/login/login2
        [HttpPost("login2")]
       [AllowAnonymous]
        public IActionResult RequestToken([FromBody]Login request)
        {
            bool found =  _context.Login.Any(info =>
            info.Name == request.Name
            && info.Password == request.Password
        );
            if (found == true)
            {
             
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name,request.Name)
                   };

                    //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SECRET_KEY"));
                    var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("YOUR-PRIVATE-RSA-KEY"));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: "yourdomain.com",
                        audience: "yourdomain.com",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);
                    var x = (new JwtSecurityTokenHandler()).WriteToken(token);


                return Ok(new
                {
                    x
                });



            }
            return BadRequest("Could not verify username and password"); 

        }
        
        //api/login/list
        
        [HttpGet("list")]
        [Authorize]
        public List<Login> Get(/*[FromHeader]String Authorization*/)
        {
            return _context.Login.ToList();
        }
      
    }
}