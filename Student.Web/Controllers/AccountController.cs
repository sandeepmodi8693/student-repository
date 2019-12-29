using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Student.Web.Models;
using Student.Web.Utility;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Student.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }

        public AccountController(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            Configuration = configuration;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model.grant_type = "password";
            var response = await ApiHelper.PostAsync<TokenResponse>(Configuration.GetSection("ApiBaseURL").Value + "token", string.Empty, new PostObject() { PostData = model });
            if (!string.IsNullOrEmpty(response.userName) && !string.IsNullOrEmpty(response.access_token))
            {
                User user = new User() { Email = response.userName, UserName = response.userName };
                HttpContext.Session.SetString("UserName", response.userName);
                HttpContext.Session.SetString("Token", response.access_token);
                return RedirectToAction("Student","Home");
            }
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var response = await ApiHelper.PostAsync<ServiceResponse<TokenResponse>>(Configuration.GetSection("ApiBaseURL").Value + "api/Account/Register", string.Empty, new PostObject() { PostData = model });
                if (response.IsSuccessful)
                {
                    HttpContext.Session.SetString("UserName", response.Data.userName);
                    HttpContext.Session.SetString("Token", response.Data.access_token);
                    return RedirectToAction("Student", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong!");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            CustomExternalLoginInfo customExternalLoginInfo = new CustomExternalLoginInfo()
            {
                ProviderKey = info.ProviderKey,
                LoginProvider = info.LoginProvider,
                Email = email
            };
            var response = await ApiHelper.PostAsync<ServiceResponse<TokenResponse>>(Configuration.GetSection("ApiBaseURL").Value + "api/Account/GetAccessToken", string.Empty, new PostObject() { PostData = customExternalLoginInfo });

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = info.LoginProvider;
            if (response.IsSuccessful)
            {
                HttpContext.Session.SetString("UserName", email);
                HttpContext.Session.SetString("Token", response.Data.access_token);
                return RedirectToAction("Student", "Home");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await _signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            throw new ApplicationException("Error loading external login information during confirmation.");
        //        }

        //        var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        //        var user = new User { UserName = model.Email, Email = model.Email, FirstName = model.Email };
        //        model.LoginProvider = info.LoginProvider;
        //        model.ProviderKey = info.ProviderKey;
        //        var response = await ApiHelper.PostAsync<ServiceResponse<TokenResponse>>(Configuration.GetSection("ApiBaseURL").Value + "api/Account/GetAccessToken", string.Empty, new PostObject() { PostData = model });

        //        if (response.IsSuccessful)
        //        {
        //            HttpContext.Session.SetString("UserName", response.Data.userName);
        //            HttpContext.Session.SetString("Token", response.Data.access_token);
        //            return RedirectToAction("Student", "Home");
        //        }
        //    }

        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View(nameof(ExternalLogin), model);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{userId}'.");
        //    }
        //    var result = await _userManager.ConfirmEmailAsync(user, code);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Lockout()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        #endregion Helpers
    }
}