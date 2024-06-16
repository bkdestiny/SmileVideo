using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class PageData<T>
    {
        private IEnumerable<T> row;

        private int total;

        private int pageSize;

        private int offset;

        private PageData() { }
        public PageData(IEnumerable<T> row, int total, int pageSize, int offset)
        {
            this.row = row;
            this.total = total;
            this.pageSize = pageSize;
            this.offset = offset;
        }
    }
}
