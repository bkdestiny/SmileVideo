using Common.EFcore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Entites
{
    public class User : IdentityUser<Guid>, IHasCreateTime, IHasDeleteTime, ISoftDelete
    {
        public bool IsDeleted { get; private set; }

        public DateTime CreateTime { get; private set; }
        public DateTime? DeleteTime { get; private set; }


        public User() { }

        public User(string userName) : base(userName)
        {
            CreateTime = DateTime.Now;
        }
        public void SoftDelete()
        {
            IsDeleted = true;
            DeleteTime = DateTime.Now;

        }
    }
}
