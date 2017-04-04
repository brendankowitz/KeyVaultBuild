using System.Linq;
using System.Threading.Tasks;
using KeyVaultBuild.Features.Authentication;
using KeyVaultBuild.Features.Config;
using KeyVaultBuild.Features.Operations;
using KeyVaultBuild.Features.Transformation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace KeyVaultBuild.Features.Build
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
            var service = new SecretService(config);
            var transformKey = new TransformKey(service);

            var files = ConfigFiles.Select(file => file.GetMetadata("Fullpath")).ToArray();

            Parallel.ForEach(files, file =>
            {
                transformKey.SaveAsFile(file);
            });

            return true;
        }
    }
}