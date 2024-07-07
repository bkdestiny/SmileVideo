using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Common.Models
{
    public class PagingData
    {
        public ICollection Rows { get;private set; }

        public int Total {  get;private set; }

        public int PageSize { get;private set; }

        public int PageIndex { get;private set; }

        private PagingData() { }
        public static PagingData Create<T>(IEnumerable<T> row,int pageSize=10, int pageIndex=1)
        {
            PagingData pageData = new PagingData();
            pageData.Rows = row.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            pageData.Total = row.Count();
            pageData.PageSize = pageSize;
            pageData.PageIndex= pageIndex;
            return pageData;
        }
    }
}
