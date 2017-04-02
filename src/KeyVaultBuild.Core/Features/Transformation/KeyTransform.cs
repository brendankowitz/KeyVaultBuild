using System.Text.RegularExpressions;

namespace KeyVaultBuild.Core.Features.Transformation
{
    public class KeyTransform
    {
        private readonly Regex _configRegex = new Regex(@"(?<keyvault>(?<=\#{keyvault:)[^}]*(?=\}))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    }
}