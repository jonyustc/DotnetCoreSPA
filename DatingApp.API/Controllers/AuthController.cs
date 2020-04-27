using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _ctx;
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(DataContext ctx,
            IAuthRepository repo,
            IConfiguration config)
        {
            _config = config;
            _repo = repo;
            _ctx = ctx;
        }

        [Authorize]
        [Route("Values")]
        public async Task<IActionResult> GetValues()
        {
            var values = await _ctx.Values.ToListAsync();
            return Ok(values);
        }

        [Route("Value/{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _ctx.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();

            if (await _repo.UserExists(userForRegisterDto.UserName))
                return BadRequest("UserName Already Exists");

            var userToCreate = new User
            {
                UserName = userForRegisterDto.UserName
            };

            var createdUser = _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = await _repo.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);

            if (userToLogin == null)
                return Unauthorized();


            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier,userToLogin.Id.ToString()),
               new Claim(ClaimTypes.Name,userToLogin.UserName)
            };

            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds 
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                Token = tokenHandler.WriteToken(token)
            });
        }
    }
}