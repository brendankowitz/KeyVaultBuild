using System.Text.RegularExpressions;

namespace KeyVaultBuild.Cli
{
    public static class StringExtensions
    {
        public static string ToSlug(this string phrase)
        {
            var s = phrase.RemoveAccent().ToLower();
            s = Regex.Replace(s, @"[^a-z0-9\s-]", ""); // remove invalid characters
            s = Regex.Replace(s, @"\s+", " ").Trim(); // single space
            s = Regex.Replace(s, @"\s", "-"); // insert hyphens
            return s.ToLower();
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}