using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Student.Web.Models;
using Student.Web.Utility;
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

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
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
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
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
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
    }
}