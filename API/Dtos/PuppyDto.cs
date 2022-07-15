namespace API.Dtos
{
    public class PuppyDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastTime { get; set; }
        public string NickName { get; set; }
        public string Story { get; set; }
        public string Role { get; set; }
        public string PreferStyle { get; set; }
        public string Gender { get; set; }
        public string Region { get; set; }
        public int Experience { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
    }
}
