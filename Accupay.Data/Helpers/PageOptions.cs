namespace AccuPay.Data.Helpers
{
    public class PageOptions
    {
        public const int DefaultPageSize = 10;

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Sort { get; set; }

        public string Direction { get; set; }

        public bool All { get; set; }

        public int Offset
        {
            get
            {
                return (PageIndex < 0 ? 0 : PageIndex) * PageSize;
            }
        }

        public static PageOptions Default
        {
            get
            {
                return new PageOptions();
            }
        }

        public PageOptions()
            : this(0)
        {
        }

        public PageOptions(
            int pageIndex,
            int pageSize = DefaultPageSize,
            string sort = "",
            string direction = "")
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Sort = sort;
            Direction = direction;
        }
    }
}
