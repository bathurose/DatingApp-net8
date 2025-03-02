using Microsoft.EntityFrameworkCore;

namespace API.Helper
{
    public class PageList<T> : List<T>
    {
        public PageList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            AddRange(items);
        }
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PageList<T>> CreatAsync(IQueryable<T> source, int pageNumber , int pageSize)
        {
            var count = await source.CountAsync();
            var item = await source.Skip((pageNumber -1 ) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(item, count, pageNumber, pageSize);
        }
    }

}

