namespace Library.EndPoint.Tools
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public List<T> Items { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public bool HasPreviousPage
        {
            get { return PageIndex > 1; }
        }

        public bool HasNextPage
        {
            get { return PageIndex < TotalPages; }
        }

        public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            int count = source.Count;
            return new PaginatedList<T>(source, count, pageIndex, pageSize);
        }
    }
}
