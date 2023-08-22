
using IdentityService.Domain;
using Zack.EventBus;

namespace IdentityService.WebAPI.Events
{
    [EventName("IdentityService.User.Created")]
    public class UserCreatedEventHandler : JsonIntegrationEventHandler<UserCreatedEvent>
    {
        private readonly IEmailSender emailSender;

        public UserCreatedEventHandler(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public override Task HandleJson(string eventName, UserCreatedEvent? eventData)
        {
            //发送密码给被用户的邮箱
            return emailSender.SendAsync(eventData.toEmail, eventData.subject, eventData.password);
        }
    }
}