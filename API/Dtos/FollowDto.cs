namespace API.Dtos
{
    public class FollowDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string PhotoUrl { get; set; }
        public string Region { get; set; }
        public int Experience { get; set; }
        public string Role { get; set; }
        public string PreferStyle { get; set; }

        public string Gender { get; set; }
    }
}
