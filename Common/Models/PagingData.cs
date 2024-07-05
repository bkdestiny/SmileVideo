using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Common.Models
{
    public class PagingData<D>
    {
        public ICollection Rows { get;private set; }

        public int Total {  get;private set; }

        public int PageSize { get;private set; }

        public int PageIndex { get;private set; }

        private PagingData() { }
        public async static Task<PagingData<D>> CreateAsync<T,D>(IQueryable<T> row,D dtoClass, int pageSize=10, int pageIndex=1)
        {
            PagingData<D> pageData = new PagingData<D>();
            pageData.Rows = await row.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
            pageData.Total = await row.CountAsync();
            pageData.PageSize = pageSize;
            pageData.PageIndex= pageIndex;
            return pageData;
        }
    }
}
