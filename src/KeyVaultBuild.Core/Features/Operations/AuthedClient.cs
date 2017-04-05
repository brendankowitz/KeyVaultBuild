using System.Threading.Tasks;
using KeyVaultBuild.Features.Authentication;
using Microsoft.Azure.KeyVault;

namespace KeyVaultBuild.Features.Operations
{
    public class AuthedClient
    {
        private readonly IAuthToken _token;
        public IKeyVaultClient KeyVault { get; }

        public AuthedClient(IKeyVaultClient keyVault)
        {
            KeyVault = keyVault;
        }

        public AuthedClient(IAuthToken token)
        {
            _token = token;
            KeyVault = new KeyVaultClient(new KeyVaultCredential(AuthenticationCallback));
        }

        private Task<string> AuthenticationCallback(string authority, string resource, string scope)
        {
            return Task.FromResult(_token.GetAuthToken(resource));
        }
    }
}