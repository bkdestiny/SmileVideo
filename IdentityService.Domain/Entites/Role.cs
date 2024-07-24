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
        public string Description { get; set; } = "";
        public Role() { 

        }
        public Role(string name,string description="")
        {
            Name = name;
            Description = description;
        }
    }
}
