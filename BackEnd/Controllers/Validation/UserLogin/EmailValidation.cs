using System.Text.RegularExpressions;

namespace SportingStatsBackEnd.Controllers.Validation.UserLogin
{
    public class EmailValidation
    {
        public static bool IsValidEmail(String email)
        {

            // Check if the email is null or empty
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            // Regular expression pattern for validating email addresses
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }

    }
}