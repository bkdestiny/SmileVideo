using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EFcore.Models
{
    public interface IHasDeleteTime
    {
        DateTime? DeleteTime { get; }
    }
}
