using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    public class UnitOfWorkAttribute : Attribute
    {
        public Type[] DbContextTypes { get; init; }
        public UnitOfWorkAttribute(params Type[] dbContextTypes)
        {
            this.DbContextTypes = dbContextTypes;
            foreach (var type in dbContextTypes)
            {
                if (!typeof(DbContext).IsAssignableFrom(type))
                {
                    throw new ArgumentException($"{type} must inherit from DbContext");
                }
            }
        }
    }
}
