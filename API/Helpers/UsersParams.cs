namespace API.Helpers
{
    //inherted paginationparams 
    public class UserParams : PaginationParams
    {
        //default setting params

        public string? CurrentUsername { get; set; }
        public string? Role { get; set; }

        public int MinExp { get; set; } = 1;
        public int MaxExp { get; set; } = 20;
        public string OrderBy { get; set; } = "LastTime";
    }
}
