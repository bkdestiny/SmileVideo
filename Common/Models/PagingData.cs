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
        public static PagingData Create<T>(IEnumerable<T> row,int pageSize=0, int pageIndex=1)
        {
            if (pageSize < 0 || pageIndex < 1)
            {
                throw new CommonException("分页参数不合法");
            }
            PagingData pageData = new PagingData();
            pageData.Total = row.Count();
            pageData.PageSize = pageSize;
            pageData.PageIndex = pageIndex;
            if (pageSize != 0)
            {
                pageData.Rows = row.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }
            else
            {
                pageData.Rows = row.ToList();
            }
            return pageData;
        }
    }
}
