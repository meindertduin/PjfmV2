namespace Pjfm.Application
{
    public class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public int Offset => (Page - 1) * PageSize;
    }
}