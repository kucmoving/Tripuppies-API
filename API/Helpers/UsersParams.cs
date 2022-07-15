namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string? CurrentUsername { get; set; }
        public string? Role { get; set; }

        public int MinExp { get; set; } = 1;
        public int MaxExp { get; set; } = 20;
        public string OrderBy { get; set; } = "LastTime";
    }
}
