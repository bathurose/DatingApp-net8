using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
//public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
    public class UsersController(IUserRepository userRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        //var users = await userRepository.GetUsersAsync();
        //var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);
        //return Ok(usersToReturn);
        //return Ok(users);
        // return NotFound();
        // retrun BadRequest();   404
        var users = await userRepository.GetMembersAsync();
       
        return Ok(users);

    }

    [HttpGet("username")] // /api/users/2 default is string
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        //var user = await userRepository.GetUserByUserNameAsync(username);
        //if (user == null)
        //{
        //    return NotFound();
        //}
        //return mapper.Map<MemberDto>(user);
        var user = await userRepository.GetMemberAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }
}
 
