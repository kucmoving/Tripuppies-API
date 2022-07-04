namespace API.Models
{
    public class UserFollow
    {
        public AppUser Follower { get; set; }
        public int FollowerId { get; set; }
        public AppUser Leader { get; set; }
        public int LeaderId { get; set; }
    }
}
