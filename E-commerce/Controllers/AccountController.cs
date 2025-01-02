using E_commerce.Models;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {
            //if(!roleManager.Roles.Any())
            //{
            //    await roleManager.CreateAsync(new("Admin"));
            //    await roleManager.CreateAsync(new("Company"));
            //    await roleManager.CreateAsync(new("Customer"));
            //}

            //await userManager.AddToRoleAsync(user, "Admin");

            ViewBag.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return View();
        }

        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Register));
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Register));
            }

            // Sign in the user with this external login provider if the user already has a login
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (signInResult.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(LoginWith2fa), new { ReturnUrl = returnUrl });
            }

            if (signInResult.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }

            // If the user does not have an account, then create one and sign in
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var user = new ApplicationUser { UserName = email, Email = email };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
            }

            ViewData["ErrorMessage"] = "Error signing in with external provider.";
            return RedirectToAction(nameof(Register));
        }



        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUserVM userVM)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    UserName = userVM.UserName,
                    Email = userVM.Email,
                    Address = userVM.Address,
                    Name = userVM.Name,
                };

                var result = await userManager.CreateAsync(user, userVM.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "يرجى ادخال كلمه سر تحتوي على احرف كبيره وصغيره وارقام وحروف مميزه");
                }
            }

            return View(userVM);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var appUserWithEmail = await userManager.FindByEmailAsync(loginVM.Account);
                var appUserWithUserName = await userManager.FindByNameAsync(loginVM.Account);

                if (appUserWithEmail != null ||  appUserWithUserName != null)
                {
                    var result = await userManager.CheckPasswordAsync(appUserWithEmail ?? appUserWithUserName, loginVM.Password);

                    if(result)
                    {
                        await signInManager.SignInAsync(appUserWithEmail ?? appUserWithUserName, loginVM.RemeberMe);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "هناك خطأ في الحساب او في كلمه السر");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "هناك خطأ في الحساب او في كلمه السر");
                }
            }

            return View(loginVM);
        }

        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            return View(model: new ApplicationUserVM()
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Address = user.Address
            });
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ApplicationUserVM userVM)
        {
            var user = await userManager.GetUserAsync(User);
            user.UserName = userVM.UserName;
            user.Email = userVM.Email;
            user.Name = userVM.Name;
            user.Address = userVM.Address;

            await userManager.UpdateAsync(user);
            TempData["success"] = "تم تحديث البيانات بنجاح";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> updatePhoto(IFormFile photo)
        {
            var user = await userManager.GetUserAsync(User);

            if (photo != null && photo.Length > 0)
            {
                // Genereate name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);

                // Save in wwwroot
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    photo.CopyTo(stream);
                }

                // Save in db
                user.photo = fileName;
                await userManager.UpdateAsync(user);
            }

            TempData["success"] = "تم تحديث البيانات بنجاح";

            return RedirectToAction("Index", "Home");
        }
    }
}
