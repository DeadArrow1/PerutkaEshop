using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Perutka.Eshop.Web.Controllers;
using Perutka.Eshop.Web.Models.ApplicationServices.Abstraction;
using Perutka.Eshop.Web.Models.Identity;
using Perutka.Eshop.Web.Models.ViewModels;
using PerutkaEshop.Models.ViewModels;
using NETCore.MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Threading.Tasks;
using System.Security.Authentication;
using Perutka.Eshop.Web.Models.database;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Collections.Generic;
using Perutka.Eshop.Web.Models.Entity;

namespace Perutka.Eshop.Web.Areas.Security.Controllers
{
    [Area("Security")]
    public class AccountController : Controller
    {
        ISecurityApplicationService security;

        readonly EshopDbContext eshopDbContext;
        IWebHostEnvironment env;

        private readonly ILogger<AccountController> logger;

        private readonly UserManager<User> userManager;
        public AccountController(ISecurityApplicationService security, UserManager<User> userManager, ILogger<AccountController> logger, EshopDbContext eshopDB, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.security = security;
            this.logger = logger;
            this.env = env;
            eshopDbContext = eshopDB;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                string[] errors = await security.Register(registerVM, Roles.Customer);

                if (errors == null)
                {
                    LoginViewModel loginVM = new LoginViewModel()
                    {
                        UserName = registerVM.UserName,
                        Password = registerVM.Password
                        //return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""), new { area = "" });
                    };

                    return await Login(loginVM);
                }
            }
            return View(registerVM);
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                loginVM.LoginFailed = !await security.Login(loginVM);

                if (loginVM.LoginFailed == false)
                {
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""), new { area = "" });
                }
            }
            return View(loginVM);
        }
        public async Task<IActionResult> Logout()
        {
            await security.Logout();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);


                    /////EMAIL SEGMENT///////
                    //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;

                    MimeMessage message = new MimeMessage();
                    IList<Email> Emails = eshopDbContext.Emails.ToList();
                    message.From.Add(new MailboxAddress("Eshop", "peeshopuser@seznam.cz"));
                    message.To.Add(MailboxAddress.Parse(model.Email));
                    message.Subject="Password reset";
                    message.Body = new TextPart("plain")
                    {
                        Text =Emails[0].Body +"\n\n" +
                        
                        "Please click the following link to change your password: " + passwordResetLink
                    };

                    //SmtpClient client = new SmtpClient();
                    using (var client = new SmtpClient())
                    {
                        try
                        {
                           

                            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                            client.Connect("smtp.gmail.com", 465, SecureSocketOptions.Auto);
                            client.Authenticate("peeshopuser@gmail.com", "KC1945WFa8/");
                            client.Send(message);

                        

                        }
                      

                        catch (System.Exception ex)
                        {
                            logger.Log(LogLevel.Error, ex.ToString());
                        }
                        finally
                        {
                            client.Disconnect(true);
                            client.Dispose();
                        }
                    }
               
                    /////EMAIL SEGMENT///////
                   
                    
                    
                    // Log the password reset link
                    logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }



    }
}
