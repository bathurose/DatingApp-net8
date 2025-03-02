using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController(IUnitOfWork unitOfWork,
        IUserRepository userRepository, IMapper mapper) : BaseApiController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUserName();
            if (username == createMessageDto.RecipientUserName)
                return BadRequest("You cannot send message to yourself");
            var sender = await userRepository.GetUserByUserNameAsync(username);
            var recipient = await userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null || sender == null 
               || recipient.UserName == null || sender.UserName == null) return BadRequest("Cannot send the message at this time");

            var message = new Message
            {
                Recipient = recipient,
                Sender = sender,
                RecipientUserName = recipient.UserName,
                SenderUserName = sender.UserName,
                Content = createMessageDto.Content
            };

            unitOfWork.MessageRepository.AddMessage(message);
            if (await unitOfWork.Complete()) return Ok(mapper.Map<MessageDto>(message));
            return BadRequest("fail to save message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser(
            [FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUserName();

            var messages = await unitOfWork.MessageRepository.GetMessageForUser(messageParams);
            Response.AddPaginationHeader(messages);
            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUserName();
            return Ok(await unitOfWork.MessageRepository.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("id")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUserName();

            var message = await unitOfWork.MessageRepository.GetMessage(id);
            if (message == null) return BadRequest("Cannot delete the message");
            if (message.SenderUserName != username || message.RecipientUserName != null)
                return Forbid();

            if (message.RecipientUserName == username) message.RecipientDeleted = true;
            if(  message.SenderUserName == username) message.SenderDeleted = true;

            if (message is { RecipientDeleted: true, SenderDeleted: true })
                unitOfWork.MessageRepository.DeleteMessage(message);

            if( await unitOfWork.Complete()) return Ok();
            return BadRequest("Problem  deleting the message");


        }
    }
}
