using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportingStatsBackEnd.Data;
using System.Threading.Tasks;

namespace SportingStatsBackEnd.Controllers.User
{
    [ApiController]
    [Route("api/auth")]
    public class GoogleLoginController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public GoogleLoginController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleCallback", "GoogleLogin");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet("api/auth/google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Redirect to login or show an error if no information is available
                return RedirectToAction("Login", "Account");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                // Redirect to your frontend or the desired authenticated page
                return Redirect("http://127.0.0.1:8080");
            }

            // Otherwise, proceed with external registration
            return RedirectToAction("RegisterExternal", "Account");
        }
    }
}
