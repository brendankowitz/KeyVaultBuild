using System.Collections.Generic;
using KeyVaultBuild.Features.Config;

namespace KeyVaultBuild
{
    public class SecretServiceBuilder
    {
        private readonly Configuration _config = new Configuration();

        public static SecretServiceBuilder Create()
        {
            return new SecretServiceBuilder();
        }

        public SecretServiceBuilder WithDirectory(string directoryId)
        {
            _config.Directory = directoryId;
            return this;
        }

        public SecretServiceBuilder WithVaultAlias(string vaultAlias, string vault)
        {
            _config.VaultAliases[vaultAlias] = vault;
            return this;
        }

        public SecretServiceBuilder WithServicePrincipal(string servicePrincipal, string secret)
        {
            _config.ServicePrincipal = servicePrincipal;
            _config.ServicePrincipalSecret = secret;
            return this;
        }

        public SecretServiceBuilder AlwaysPromptInteractiveAuth(bool promptAuth)
        {
            _config.AlwaysPromptInteractiveAuth = promptAuth;
            return this;
        }

        public ISecretService Build()
        {
            return new SecretService(_config);
        }
    }
}