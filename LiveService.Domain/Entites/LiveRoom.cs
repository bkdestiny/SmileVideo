using Common.EFcore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveService.Domain.Entites
{
    public class LiveRoom : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Title { get; set; } = "";

        public Guid? CoverFile { get; set; }

        private LiveRoom() { }

        public LiveRoom(Guid userId)
        {
            UserId = userId;
        }
    }
}
