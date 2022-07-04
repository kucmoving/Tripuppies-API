namespace API.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }    
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastTime { get; set; }=DateTime.UtcNow;
        public string NickName { get; set; } 
        public string Story { get; set; } = string.Empty;
        public string Role { get; set; } 
        public string PreferStyle { get; set; } 
        public int Experience { get; set; }
        public string Gender { get; set;} 
        public string Region { get; set; } 
        public ICollection<UserFollow> FollowingOther { get; set; }
        public ICollection <UserFollow> FollowedByOther { get; set; }
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
