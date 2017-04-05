using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KeyVaultBuild.Features.Transformation
{
    public class TransformKeys
    {
        private readonly ISecretService _secretService;
        private static readonly Regex KeyRegex = new Regex(@"(?<keyvault>\#{keyvault:[a-zA-Z0-9:\-]*})", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        public TransformKeys(ISecretService secretService)
        {
            _secretService = secretService;
        }

        public static bool IsKeySyntax(string keySyntax)
        {
            return KeyRegex.IsMatch(keySyntax);
        }

        public string ReplaceKeys(string content)
        {
            return KeyRegex.Replace(content, Evaluator);
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