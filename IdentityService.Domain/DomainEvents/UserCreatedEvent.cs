using MediatR;

namespace IdentityService.Domain.DomainEvents
{
    public record UserCreatedEvent(Guid Id,string userName,string password,string phoneNumber) : INotification;
}
