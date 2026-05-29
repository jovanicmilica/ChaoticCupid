using System;
using System.Linq;

namespace ChaoticCupid.Service.Services
{
    public static class ValidationHelper
    {
        public static bool ValidateUser(string username, string city, int age, string phone)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty!");
                return false;
            }

            if (!username.All(char.IsLetterOrDigit))
            {
                Console.WriteLine("Username can only contain letters and digits!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                Console.WriteLine("City cannot be empty!");
                return false;
            }

            if (!city.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                Console.WriteLine("City can only contain letters!");
                return false;
            }

            if (age <= 0 || age > 150)
            {
                Console.WriteLine($"Age is not valid! Entered: {age}. Allowed: 1-150");
                return false;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                Console.WriteLine("Phone number cannot be empty!");
                return false;
            }

            if (!phone.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' '))
            {
                Console.WriteLine("Phone number can only contain digits, +, - and spaces!");
                return false;
            }

            return true;
        }
    }
}