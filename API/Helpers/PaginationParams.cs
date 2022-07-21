namespace API.Helpers
{
    public class PaginationParams
    {
        //default properties 
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        // default get 10, if client choose larger, return larger
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}


