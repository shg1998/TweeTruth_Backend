namespace Service.WebFramework.Pagination
{
    public class PaginationParameters
    {
        private const int MaxPageSize = 101;
        public int PageNumber { get; set; } = 1;

        private int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
