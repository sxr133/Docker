using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SportingStatsBackEnd.Data;
using SportingStatsBackEnd.Models;

namespace SportingStatsBackEnd.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SignupController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Signup([FromBody] UserSignup request)
        {
            try
            {
                // Validate request data
                if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { Message = "All fields are required" });
                }

                // Check if user with email already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    return Conflict(new { Message = "User with this email already exists" });
                }

                // Generate a verification token
                var token = GenerateVerificationToken();

                Console.WriteLine("New user signing up is set to: {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", request.FirstName, request.LastName,
                    request.StreetAddress1, request.StreetAddress2, request.City, request.State_Prov, request.Zip_Post_Cd, request.Country,
                    request.Email, request.Password, request.VerificationToken, request.IsEmailVerified);

                // Create new user
                var newUserSignup = new UserSignup
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    StreetAddress1 = request.StreetAddress1,
                    StreetAddress2 = request.StreetAddress2,
                    City = request.City,
                    State_Prov = request.State_Prov,
                    Zip_Post_Cd = request.Zip_Post_Cd,
                    Country = request.Country,
                    Email = request.Email,
                    Password = request.Password,
                    VerificationToken = token, // Add the verification token
                    IsEmailVerified = false   // Set to false initially
                    // You might want to hash the password before storing it
                };


                // Send verification email
                SendVerificationEmail(request.FirstName, request.LastName, request.Email, token);

                _context.UserSignups.Add(newUserSignup);
                _context.SaveChanges();

                // Create a new user instance
                var newUser = new SportingStatsBackEnd.Models.User
                {
                    Email = request.Email,
                    Password = request.Password,
                    // You can add other properties here
                };

                // Add the user to the Users table
                _context.Users.Add(newUser);

                // Save changes to the database
                _context.SaveChanges();

                return Ok(new { Message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred" });
            }
        }
        private string GenerateVerificationToken()
        {
            Console.WriteLine("token created");
            // Generate a unique verification token (you can use GUID or any other method)
            return Guid.NewGuid().ToString();
        }

        private void SendVerificationEmail(string firstName, string lastName, string email, string token)
        {
            Console.WriteLine("Getting ready to send email out to {0} with token {1} to {2} {3}", email, token, firstName, lastName);
            String fullName = firstName + " " + lastName;
            try
            {
                // Configure SMTP client for Gmail
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("sterous@gmail.com", "bhnwdcgromgzjkwl");

                    // Construct the email message
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Steve Rousseau", "sterous@gmail.com"));
                    message.To.Add(new MailboxAddress(fullName, email));
                    message.Subject = "Verify Your Email Address";
                    var builder = new BodyBuilder();
                    builder.HtmlBody = $"Please click the following link to verify your email address: <a href='https://localhost:7102/EmailVerification/verify?token={token}'>Verify Email</a>";
                    message.Body = builder.ToMessageBody();

                    // Send the email
                    client.Send(message);
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}