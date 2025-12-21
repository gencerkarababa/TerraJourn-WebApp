using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BCrypt.Net;

namespace TerraJourn.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyPassword(string enteredPassword, string hashedPasswordFromDb)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPasswordFromDb);
        }
    }
}
