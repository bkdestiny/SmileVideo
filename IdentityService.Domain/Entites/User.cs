using Common.EFcore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.Entites
{
    public class User : IdentityUser<Guid>, IHasCreateTime, IHasDeleteTime
    {
        public Guid Avatar { get; set; }

        public DateTime CreateTime { get; private set; }
        public DateTime? DeleteTime { get; private set; }


        private User() { }
        public User(Guid id,string userName, string? phoneNumber = "", string? email = "") : base(userName)
        {
            this.Id = id;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            CreateTime = DateTime.Now;
        }
        public User(string userName,string? phoneNumber="",string? email="") : base(userName)
        {
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            CreateTime = DateTime.Now;
        }
    }
}
