using Microsoft.AspNetCore.Identity.UI.Services;

namespace SampleEcommerceWebsite.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Email Sender Logic Goes here
            return Task.CompletedTask;
        }
    }
}
