using API.Entities;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController(UserManager<AppUser> userManager) : BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsesWithRoles()
        {
            var user = await userManager.Users
            .OrderBy(x=>x.UserName)
            .Select(x=> new{
                x.Id,
                Username = x.UserName,
                Role = x.UserRoles.Select(x=>x.Role.Name).ToList()
            }).ToListAsync();
            return Ok(user);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            if(string.IsNullOrEmpty(roles)) return BadRequest("You must select one role");
             var selectedRoles = roles.Split(",").ToArray();
             var user = await userManager.FindByNameAsync(username);
             if(user == null) return BadRequest("User not found");
            
             var userRoles = await userManager.GetRolesAsync(user);

             var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

             if(!result.Succeeded) return BadRequest("Failed to add user role");

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

              if(!result.Succeeded) return BadRequest("Failed to remove user role");   
              return Ok(await userManager.GetRolesAsync(user));

        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }

    }
}