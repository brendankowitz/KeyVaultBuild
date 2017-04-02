using System.Linq;
using KeyVaultBuild.Core.Features.Authentication;
using KeyVaultBuild.Core.Features.Config;
using KeyVaultBuild.Core.Features.Operations;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace KeyVaultBuild.Core.Features.Build
{
    public class KeyVaultReplaceTask : AppDomainIsolatedTask
    {
        [Required]
        public ITaskItem[] ConfigFiles { get; set; }

        public string ClientId { get; set; }

        public string Secret { get; set; }

        public string DirectoryId { get; set; }

        public string OverrideVaults { get; set; }

        public override bool Execute()
        {
            var config = new Configuration { Directory = DirectoryId };
            var auth = new InteractiveAuthToken(config);
            var client = new AuthedClient(auth);

            var files = ConfigFiles.Select(file => file.GetMetadata("Fullpath")).ToArray();

            return true;
        }
    }
}