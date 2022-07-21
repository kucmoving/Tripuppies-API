namespace API.Helpers
{
    //inherted paginationparams
    public class MessageParams : PaginationParams
    {
        public string UserName { get; set; }
        public string Container { get; set; } = "Unread";
    }

}
