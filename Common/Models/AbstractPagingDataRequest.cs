using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public abstract class AbstractPagingDataRequest
    {
        public int PageSize { get; set; } = 0;

        public int PageIndex { get; set; } = 1;
    }
}
