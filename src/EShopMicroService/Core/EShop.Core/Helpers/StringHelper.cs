using System;
namespace EShop.Core.Helpers
{
    public static class StringHelper
    {
        public static string GetFullName(string firstName, string lastName)
        {
            return $"{firstName.Substring(0, 1).ToUpper()}{firstName.Remove(0, 1).ToLower()} {lastName.Substring(0, 1).ToUpper()}{lastName.Remove(0, 1).ToLower()}";
        }

        public static string GetFirstName(string firstName)
        {
            return $"{firstName.Substring(0, 1).ToUpper()}{firstName.Remove(0, 1).ToLower()}";
        }
    }
}