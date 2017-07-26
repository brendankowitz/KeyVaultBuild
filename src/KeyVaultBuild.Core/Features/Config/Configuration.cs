using System.Collections.Generic;

namespace KeyVaultBuild.Features.Config
{
    public class Configuration
    {
        private string _directory;
        private string _servicePrincipal;
        private string _servicePrincipalSecret;
        private bool _alwaysPromptInteractiveAuth;

        public string Directory
        {
            get { return _directory; }
            set { _directory = value?.Trim(); }
        }

        public string ServicePrincipal
        {
            get { return _servicePrincipal; }
            set { _servicePrincipal = value?.Trim(); }
        }

        public string ServicePrincipalSecret
        {
            get { return _servicePrincipalSecret; }
            set { _servicePrincipalSecret = value?.Trim(); }
        }

        public bool AlwaysPromptInteractiveAuth
        {
            get { return _alwaysPromptInteractiveAuth; }
            set { _alwaysPromptInteractiveAuth = value; }
        }

        public IDictionary<string,string> VaultAliases { get; } = new Dictionary<string, string>();
    }
}