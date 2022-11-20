namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = "admin@myCompany.com";
        private string _mailFrom = "noreply@myCompany.com";

        public void Send(string subject, string body)
        {
            Console.WriteLine($"Send email from {_mailFrom} to {_mailTo}, with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject:{subject}");
            Console.WriteLine($"Body: {body}");
        }
    }
}
