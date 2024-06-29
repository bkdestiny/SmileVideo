using Common.Sms;
using MediatR;

namespace IdentityService.Domain.DomainEvents
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly ISms sms;

        public UserCreatedEventHandler(ISms sms)
        {
            this.sms = sms;
        }

        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("发送短信通知用户初始密码:" + notification.ToString());
            return Task.CompletedTask;
        }
    }
}
