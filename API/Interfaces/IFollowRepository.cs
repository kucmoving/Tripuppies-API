using API.Dtos;
using API.Models;

namespace API.Interfaces
{
    public interface IFollowRepository
    {
        Task<UserFollow> GetUserFollowInfo(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserFollowing(int userId);
        Task<IEnumerable<FollowDto>> GetUserFollowDetails(string predicate, int userId);
    }
}
