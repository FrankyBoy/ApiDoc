using System;

namespace ApiDoc.Utility
{
    public static class StringExtensions
    {
        public static string ToWikiUrlString(this string input)
        {
            return input.Replace(' ', '_');
        }

        public static string FromWikiUrlString(this string input)
        {
            return input.Replace('_', ' ');
        }

        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}