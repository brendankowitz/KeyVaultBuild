using System.Text.RegularExpressions;

namespace KeyVaultBuild.Core.Features.Transformation
{
    public class KeyTransform
    {
        private static readonly Regex _configRegex = new Regex(@"(?<keyvault>(?<=\#{keyvault:)[^}]*(?=\}))", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        public static bool IsKeySyntax(string keySyntax)
        {
            return _configRegex.IsMatch(keySyntax);
        }
    }
}