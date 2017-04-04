using System.Threading.Tasks;
using KeyVaultBuild.Features.Config;
using Microsoft.Azure.KeyVault;

namespace KeyVaultBuild.Features.Operations
{
    public class ReadKey
    {
        public string Vault { get; }
        public string Key { get; }
        private readonly AuthedClient _client;
        private string _cachedSecret;

        public ReadKey(AuthedClient client, string vault, string key)
        {
            Vault = vault;
            Key = key;
            _client = client;
        }

        public async Task<string> ExecuteAsync()
        {
            if (string.IsNullOrEmpty(_cachedSecret))
            {
                Log.Information($"Reading key '{Key}' from '{Vault}'");
                var secret = await _client.KeyVault.GetSecretAsync($"https://{Vault}.vault.azure.net/", Key);
                _cachedSecret = secret.Value;
            }
            return _cachedSecret;
        }
    }
}