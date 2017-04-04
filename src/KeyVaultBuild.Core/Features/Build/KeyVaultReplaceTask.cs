using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public string VaultAliases { get; set; }

        public string DebugTask { get; set; }

        public override bool Execute()
        {
            var debug = string.Equals("true", DebugTask, StringComparison.OrdinalIgnoreCase);
            Config.Log.Information = x => Log.LogMessage(x);
            Config.Log.Error = (ex, m) => Log.LogError("Error while processing secrets from keyvault. " + Environment.NewLine + ex);

            try
            {
                if (debug && !Debugger.IsAttached)
                {
                    Debugger.Launch();
                    Debugger.Break();
                }

                var service = SecretServiceBuilder.Create()
                    .WithDirectory(DirectoryId)
                    .WithServicePrincipal(ClientId, Secret);

                if (!string.IsNullOrEmpty(VaultAliases))
                {
                    foreach (var alias in VaultAliases.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var pair = alias.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        service.WithVaultAlias(pair.First(), pair.Last());
                        Log.LogWarning("Overriding vault '{0}' with '{1}'", pair.First(), pair.Last());
                    }
                }

                var transformKey = new TransformKeys(service.Build());
                var files = ConfigFiles.Select(file => file.GetMetadata("Fullpath")).ToArray();

                Parallel.ForEach(files, file =>
                {
                    transformKey.SaveAsFile(file);
                });
            }
            catch(Exception ex)
            {
                Config.Log.Error(ex, "Error occured processing secrets. ");
                if (debug)
                    Debugger.Break();
                return false;
            }

            return true;
        }
    }
}