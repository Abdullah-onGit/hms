using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using hms.Models;
using hms.BO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace hms.Controllers
{
    public class AccountController : Controller
    {
        private AccountBO accountBO;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AccountBO accountBO, ILogger<AccountController> logger)
        {
            _logger = logger;
            this.accountBO = accountBO;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserMaster user)
        {
            IActionResult actionResult = View();
            var rec = accountBO.ValidateUser(user);
            if (rec != null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("UserName", user.UserName));

                var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);
                HttpContext.Session.SetString(hmsConstants.userName,rec.UserName);
                HttpContext.Session.SetInt32(hmsConstants.hmsTenatAuotId,rec.HmsTenantAutoId);
                actionResult = RedirectToAction("Dashboard","Dashboard");
            }
            else
            {
                ViewBag.msg = "Invalid User Name or Password";

            }
            return actionResult;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }


    }
}
