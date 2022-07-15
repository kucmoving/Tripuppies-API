using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class MessageController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _imapper;

        public MessageController(IUserRepository userRepository,
            IMessageRepository messageRepository, IMapper imapper)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _imapper = imapper;
        }

        [HttpGet]     //MessageParams : UserName, Container
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUsername();
            var messages = await _messageRepository.GetMessagesForUser(messageParams);
            // http response
            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize,
                messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _messageRepository.GetMessageThread(currentUsername, username));
        }



        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientName.ToLower())
                return BadRequest("please chat with others");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientName);

            if (recipient == null) return NotFound();
            var message = new Message                 //to  rganize the details to pass it to message 
            {
                Sender = sender,
                Recipient = recipient,
                SenderName = sender.UserName,
                RecipientName = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync())
                return Ok(_imapper.Map<MessageDto>(message));

            return BadRequest("Something go wrong.Cannot send the message now.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var message = await _messageRepository.GetMessage(id);

            if (message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDel = true;

            if (message.Recipient.UserName == username) message.RecipientDel = true;

            if (message.SenderDel && message.RecipientDel)
                _messageRepository.DelMessage(message);

            if (await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting the message");
        }

    }
}