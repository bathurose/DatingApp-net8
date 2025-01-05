using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")] //account/register
 //       public async Task<ActionResult<AppUser>> Register([FromQuery]string username, string password)
 //       public async Task<ActionResult<AppUser>> Register([FromBody] string username, string password) //default
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if( await UserExits(registerDto.Username) ) return BadRequest("User is taken");
            using var hmac = new HMACSHA512();
            return Ok();
            //var user = new AppUser
            //{
            //    UserName = registerDto.Username.ToLower(),
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //    PasswordSalt = hmac.Key

            //};

            //context.Users.Add(user);
            //await context.SaveChangesAsync();

            //return new UserDto
            //{
            //    Username = user.UserName,
            //    Token = tokenService.CreateToken(user),
            //};
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(
                x => x.UserName == loginDto.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user),
            };
        }

        private async Task<bool> UserExits(string username)
        {
            //var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == username);
            //if (user == null)
            //{
            //    return false;
            //}
            //return true;
        
            return await context.Users.AnyAsync(x => x.UserName == username); //Bob!=bob 
        }

    }
}
