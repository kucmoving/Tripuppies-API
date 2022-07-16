
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _imapper;
        private readonly IPhotoService _photoService;

        public UserController(IUserRepository userRepository, 
            IMapper imapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _imapper = imapper;
            _photoService = photoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PuppyDto>>> GetUsers([FromQuery] UserParams  userParams)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = user.UserName;

     //       if (string.IsNullOrEmpty(userParams.Role))
     //           userParams.Role = user.Role == "Traveller" ? "Guide" : "Traveller";

            var users = await _userRepository.GetPuppiesAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages);

            return Ok(users);
        }


        //       [HttpGet("{username}")]
        //       public async Task<ActionResult<PuppyDto>> GetUser(string username)
        //       {
        //           return await _userRepository.GetPuppyAsync(username);
        //        }

        [HttpGet("{username}", Name = "GetUser")]
        //[HttpGet("{username}")]

        public async Task<ActionResult<PuppyDto>> GetUser(string username)
        {
            var result = await _userRepository.GetPuppyAsync(username);
            return Ok(result);
        }


        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
                //return _mapper.Map<PhotoDto>(photo);
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _imapper.Map<PhotoDto>(photo));

            return BadRequest("problem adding photo");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(PuppyEditDto puppyEditDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            _imapper.Map(puppyEditDto, user);
            _userRepository.Update(user);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You cannot delete your main photo");
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to delete the photo");
        }



    }
}





