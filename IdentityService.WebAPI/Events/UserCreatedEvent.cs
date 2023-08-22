namespace IdentityService.WebAPI.Events
{
    public record UserCreatedEvent(Guid Id, string subject, string password, string toEmail);
}
