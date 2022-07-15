using API.Dtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IFollowRepository _followRepository;

        public FollowController(IUserRepository userRepository, IFollowRepository followRepository)
        {
            _userRepository = userRepository;
            _followRepository = followRepository;
        }

        //

        [HttpPost("{username}")]
        public async Task<ActionResult> AddFollow(string username)
        {
            //to define the element 
            var peopleIFollow = await _userRepository.GetUserByUsernameAsync(username);              //get leader name 
            if (peopleIFollow == null) return NotFound();

            var followerId = User.GetUserId();                                                                                 //get leader id 

            var follower = await _followRepository.GetUserFollowing(followerId);                                 //to define user following
            if (follower.UserName == username) return BadRequest("You cannot follow yourself");


            var userFollow = await _followRepository.GetUserFollowInfo(followerId, peopleIFollow.Id);   //to build the relationship
            if (userFollow != null) return BadRequest("You are in the relationship already.");                // to check 

            //to build a field in UserFollow
            userFollow = new UserFollow
            {
                FollowerId = followerId,
                LeaderId = peopleIFollow.Id
            };

            //to save
            follower.FollowingOther.Add(userFollow);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to follow");

        }

       
       //to query user following or followedby information 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FollowDto>>> GetUserLikes(string predicate)
        {
            var users = await _followRepository.GetUserFollowDetails(predicate, User.GetUserId());
            return Ok(users);
        }
    }
}
