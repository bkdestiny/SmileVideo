using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Entites
{
    public class Role : IdentityRole<Guid>
    {
        public Role() { 

        }
        public Role(string name)
        {
            Name = name;
        }
    }
}
