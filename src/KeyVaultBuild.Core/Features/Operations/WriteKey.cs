using System.Threading.Tasks;
using KeyVaultBuild.Features.Config;
using Microsoft.Azure.KeyVault;

namespace KeyVaultBuild.Features.Operations
{
    public class WriteKey
    {
        public string Vault { get; }
        private readonly AuthedClient _client;

        public WriteKey(AuthedClient client, string vault)
        {
            Vault = vault;
            _client = client;
        }

        public async Task Set(string key, string value)
        {
            Log.Information($"Uploading key '{key}' to '{Vault}'.");
            await _client.KeyVault.SetSecretAsync($"https://{Vault}.vault.azure.net/", key, value);
        }
    }
}