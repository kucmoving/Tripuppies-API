using API.Data;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class FollowRepository : IFollowRepository
    {
        private readonly DataContext _dataContext;

        public FollowRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
       
        public async Task<UserFollow> GetUserFollowInfo(int followerId, int leaderId)
        {
            return await _dataContext.UserFollows.FindAsync(followerId, leaderId);
        }

        // get who follow user 

        public async Task<AppUser> GetUserFollowing(int userId)
            {
            return await _dataContext.Users
                .Include(x => x.FollowingOther)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<IEnumerable<FollowDto>> GetUserFollowDetails(string predicate, int userId)
        {
            //use joint query to make
            //Asqueryable will not execute instantly, but super efficient
            var users = _dataContext.Users.OrderBy(u => u.UserName).AsQueryable();
            var follows = _dataContext.UserFollows.AsQueryable();

            //set the condition of predicate
            if (predicate == "following")  //user following
            {
                follows = follows.Where(u => u.FollowerId == userId);
                users = follows.Select(u => u.Leader);
            }

            if (predicate == "followedBy")  //user followed by
            {
                follows = follows.Where(i => i.LeaderId == userId);
                users = follows.Select(i => i.Follower);
            }

            return await users.Select(user => new FollowDto
            {
                Username = user.UserName,
                Nickname = user.NickName,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                Region = user.Region,
                Experience = user.Experience,
                Role = user.Role,
                PreferStyle = user.PreferStyle,
                Id = user.Id
            }).ToListAsync();
        }
    }
}
