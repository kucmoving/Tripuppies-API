using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string PreferStyle { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Region { get; set; }

    }
}
