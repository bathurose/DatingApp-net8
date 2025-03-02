using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork,
        ITokenService tokenService, IMapper mapper) : BaseApiController
    {
        [HttpPost("register")] //account/register
 //       public async Task<ActionResult<AppUser>> Register([FromQuery]string username, string password)
 //       public async Task<ActionResult<AppUser>> Register([FromBody] string username, string password) //default
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if( await UserExits(registerDto.Username!) ) return BadRequest("User is taken");
            
            using var hmac = new HMACSHA512();

            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username!.ToLower();
           

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);

            var assetPath = "https://randomuser.me/api/portraits/lego/1.jpg";
            var photo = new Photo
            {
                Url = assetPath,    
                IsMain = true,
            };

            user.Photos.Add(photo);

            if (await unitOfWork.Complete())
                return new UserDto
                {
                    Username = user.UserName,
                    KnownAs = user.KnownAs,
                    Gender = user.Gender,
                    Token = await tokenService.CreateToken(user),
                    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                };
            return BadRequest("Register new account fail");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync
                (x => x.NormalizedUserName == loginDto.UserName.ToUpper());

            if (user == null || user.UserName == null) return Unauthorized("Invalid username");

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!result) return Unauthorized();



            return new UserDto
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
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
        
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper()); //Bob!=bob 
        }

    }
}
