using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportingStatsBackEnd.Data;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SportingStatsBackEnd.Controllers.User
{
    [ApiController]
    [Route("api/auth")]
    public class FacebookLoginController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegularLoginController> _logger;

        // Constructor for injecting SignInManager and UserManager
        public FacebookLoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<RegularLoginController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        // Starts the Facebook login flow
        [HttpGet("facebook")]
        
        public IActionResult FacebookLogin()
        {
            _logger.LogInformation("Facebook login endpoint was accessed.");
            // Set the redirect URL after Facebook login
            var redirectUrl = Url.Action("FacebookCallback", "FacebookLogin"); // Ensure "FacebookLogin" matches the controller name
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return Challenge(properties, "Facebook"); // Initiates the external login process with Facebook
        }

        // Handles the callback after Facebook redirects the user
        [HttpGet("api/auth/facebook/callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            // Get the external login info (this will be populated after successful authentication with Facebook)
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return BadRequest("External login information is not available.");
            }

            // Attempt to sign in the user with the external login info
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                // If login succeeded, redirect to the home page or desired page
                return Redirect("http://127.0.0.1:5000/home"); // Replace with your actual redirect URL
            }
            else
            {
                // User doesn't exist, create a new account
                var user = new IdentityUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };

                var createResult = await _userManager.CreateAsync(user);

                if (createResult.Succeeded)
                {
                    // Link the external login with the new user
                    var loginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
                    if (loginResult.Succeeded)
                    {
                        return Redirect("http://127.0.0.1:8080/home"); // Replace with your actual redirect URL
                    }
                }

                // If creating the user fails, return an error
                return BadRequest("External login failed.");
            }
        }
    }
}
