using E_commerce.Utility;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IEmailSender emailSender;

        public CheckoutController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> Success()
        {
            await emailSender.SendEmailAsync("test@gmail.com", "success pay", "bla bla bla");
            return RedirectToAction("Index", "Home");
        }
    }
}
