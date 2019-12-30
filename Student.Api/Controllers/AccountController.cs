using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Student.Api.Models;
using Student.Api.Providers;
using Student.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Student.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            ServiceResponse serviceResponse = new ServiceResponse();

            if (!result.Succeeded)
            {
                serviceResponse.IsSuccessful = false;
                serviceResponse.Data = new TokenResponse() { userName = result.Errors.First().ToString() };
                return Ok(serviceResponse);
            }
            ClaimsIdentity oAuthIdentity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            DateTime currentUtc = DateTime.UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(365));
            string accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
            Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            TokenResponse token = new TokenResponse()
            {
                access_token = accessToken,
                token_type = "bearer",
                expires = currentUtc.Add(TimeSpan.FromDays(365)).ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'"),
                issued = currentUtc.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"),
                expires_in = TimeSpan.FromDays(365).TotalSeconds.ToString(),
                userName = user.UserName,
                userId = user.Id

            };
            serviceResponse.Data = token;
            serviceResponse.IsSuccessful = true;
            return Ok(serviceResponse);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("GetAccessToken")]
        public async Task<IHttpActionResult> GetAccessToken(RegisterExternalUserModel model)
        {
            if (model.Key.Equals(AppConstants.TokenRequest))
            {
                var user = UserManager.FindByEmail(model.Email);
                if (user == null)
                {
                    user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                    await UserManager.CreateAsync(user);
                    if (!string.IsNullOrEmpty(model.LoginProvider) && !string.IsNullOrEmpty(model.ProviderKey))
                    {
                        UserLoginInfo loginInfo = new UserLoginInfo(model.LoginProvider, model.ProviderKey);
                        await UserManager.AddLoginAsync(user.Id, loginInfo);
                    }
                }
                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
                DateTime currentUtc = DateTime.UtcNow;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(365));
                string accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
                Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                TokenResponse token = new TokenResponse()
                {
                    access_token = accessToken,
                    token_type = "bearer",
                    expires = currentUtc.Add(TimeSpan.FromDays(365)).ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'"),
                    issued = currentUtc.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"),
                    expires_in = TimeSpan.FromDays(365).TotalSeconds.ToString(),
                    userName = user.UserName,
                    userId = user.Id

                };
                ServiceResponse serviceResponse = new ServiceResponse()
                {
                    Data = token,
                    IsSuccessful = true
                };
                return Ok(serviceResponse);
            }
            else
            {
                ServiceResponse serviceResponse = new ServiceResponse()
                {
                    Data = null,
                    IsSuccessful = false
                };
                return Ok(serviceResponse);
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
                _signInManager.Dispose();
                _signInManager = null;
            }
            base.Dispose(disposing);
        }
    }
}
