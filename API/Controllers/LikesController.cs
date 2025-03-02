using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(IUnitOfWork  unitOfWork) : BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var userId = User.GetUseId();

            if (userId == targetUserId) return BadRequest("You can not like yourself");

            var existingLike =  await unitOfWork.LikesRepository.GetUserLike(userId, targetUserId);

            if (existingLike == null)
            {
                
                var like = new UserLike
                {
                    SourceUserId = userId,
                    TargetUserUId = targetUserId
                };
                unitOfWork.LikesRepository.AddLike(like);   
            }
            else
            {
                unitOfWork.LikesRepository.DeleteLike(existingLike);   
            }

            if( await unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to update like");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {

            likesParams.UserId = User.GetUseId() ;
 
            var users = await unitOfWork.LikesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }

    }
}
