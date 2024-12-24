using E_commerce.Models;
using E_commerce.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;

        public CheckoutController(IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Success()
        {
            var user = await userManager.GetUserAsync(User);

            await emailSender.SendEmailAsync(await userManager.GetEmailAsync(user), "success pay", $"<html><body> <h1>bla bla bla</h1> </body></html>");
            return RedirectToAction("Index", "Home");
        }
    }
}
