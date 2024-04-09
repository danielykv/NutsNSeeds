using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace AsifNutsNSeeds.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendEmail(string msg,string name,string email,string phone)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);
            var enableSSL = bool.Parse(smtpSettings["EnableSSL"]);
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];

            var message = new MailMessage();
            message.From = new MailAddress("etomer9@gmail.com"); // Set the sender's email address
            message.To.Add("etomer9@gmail.com");
            message.Subject = "New client message";
            message.Body = $"Name: {name}\n\nEmail:{email}\n\nPhone Number:{phone}\n\nMessage:{msg} ";

            using (var smtp = new SmtpClient(host, port))
            {
                smtp.EnableSsl = enableSSL;
                smtp.Credentials = new NetworkCredential(username, password);

                await smtp.SendMailAsync(message);
            }

            return View("Index");
        }
    }
}
