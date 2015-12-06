using System;

namespace EnterpriseApps.Portable.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("String cannot be null or empty.");
            return input[0].ToString().ToUpper() + input.Substring(1);
        }
    }
}
