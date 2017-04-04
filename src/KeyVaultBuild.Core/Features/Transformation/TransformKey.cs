using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KeyVaultBuild.Features.Transformation
{
    public class TransformKey
    {
        private readonly ISecretService _secretService;
        private static readonly Regex s_configRegex = new Regex(@"(?<keyvault>(?<=\#{keyvault:)[^}]*(?=\}))", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        public TransformKey(ISecretService secretService)
        {
            _secretService = secretService;
        }

        public static bool IsKeySyntax(string keySyntax)
        {
            return s_configRegex.IsMatch(keySyntax);
        }

        public string ReplaceKeys(string content)
        {
            return s_configRegex.Replace(content, Evaluator);
        }

        private string Evaluator(Match match)
        {
            var key = match.Groups["keyvault"].Value;
            var keyVaultKey = _secretService.ResolveSingleKey(key);
            var result = Task.Factory.StartNew(() => keyVaultKey.ExecuteAsync()).Unwrap().GetAwaiter().GetResult();
            return match.Result(result);
        }
    }
}