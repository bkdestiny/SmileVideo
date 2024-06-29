using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MediatR.Models
{
    public record DomainEvent<T>(Guid Id, long Timestamp, string Source, T Data) : INotification;
}
