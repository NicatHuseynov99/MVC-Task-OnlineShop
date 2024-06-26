﻿using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.ViewModels.Account;
using static MvsTaskOnlineShop.Utilities.Helpers.Helper;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MvsTaskOnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AppUser newUser = new()
            {
                FullName = model.FullName,
                UserName = model.Username,
                Email = model.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(model);
            }
            await _userManager.AddToRoleAsync(newUser, UserRoles.Member.ToString());
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View();
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View();
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MvcTaskOnlineShop", "nicat1234554321@gmail.com"));
            message.To.Add(new MailboxAddress(user.FullName, user.Email));
            message.Subject = " MvcTaskOnlineShop - Reset Password";

            string emailBody = string.Empty;
            using (StreamReader streamReader = new StreamReader(Path.Combine(_env.WebRootPath, "templates", "mail.html")))
            {
                emailBody = streamReader.ReadToEnd();
            }
            string forgotPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = Url.Action("changepassword", "dashboard", new { Id = user.Id, token = forgotPasswordToken }, Request.Scheme);
            emailBody = emailBody.Replace("{{url}}", $"{url}");
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailBody };


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("nicat1234554321@gmail.com", "eogu wfxg pjcw sgel");
            smtp.Send(message);
            smtp.Disconnect(true);

            return RedirectToAction("Login");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError("", $"User with id {model.Id} was not found");
                return View();
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Could not change user`s password");
                return View();
            }
            return RedirectToAction("Login");
        }
        //public async Task CreateRole()
        //{
        //    foreach (var item in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(item.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
        //        }
        //    }
        //}
        //public async Task CreateAdmin()
        //{
        //    var user = await _userManager.FindByEmailAsync("nicat-59@mail.ru");
        //    if (user != null)
        //    {
        //        await _userManager.RemoveFromRoleAsync(user, UserRoles.Member.ToString());
        //        await _userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
        //    }
        //}
    }
}
