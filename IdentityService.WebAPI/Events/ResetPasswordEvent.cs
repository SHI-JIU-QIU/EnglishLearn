namespace IdentityService.WebAPI.Events
{
    public record ResetPasswordEvent(Guid Id, string subject, string password, string toEmail);
}
