using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KeyVaultBuild.Features.Transformation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

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

        public string AlwaysPromptInteractiveAuth { get; set; }

        public override bool Execute()
        {
            var debug = string.Equals("true", DebugTask?.Trim(), StringComparison.OrdinalIgnoreCase);
            var promptAuth = string.Equals("true", AlwaysPromptInteractiveAuth?.Trim(), StringComparison.OrdinalIgnoreCase);
            Config.Log.Information = x => Log.LogWarning(x);
            Config.Log.Error = (ex, m) => Log.LogError("Error while processing secrets from keyvault. " + Environment.NewLine + ex);

            try
            {
                if (debug && !Debugger.IsAttached)
                {
                    Debugger.Launch();
                    Debugger.Break();
                }

                if (promptAuth)
                {
                    Log.LogWarning("KeyVaultBuildTask: Set to always prompting for auth information.");
                }

                var service = SecretServiceBuilder.Create()
                    .AlwaysPromptInteractiveAuth(promptAuth)
                    .WithDirectory(DirectoryId)
                    .WithServicePrincipal(ClientId, Secret);

                if (!string.IsNullOrEmpty(VaultAliases))
                {
                    foreach (var alias in VaultAliases?.Trim().Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries))
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
                TokenCache.DefaultShared.Clear();
                Config.Log.Error(ex, "Error occured processing secrets. ");
                if (debug)
                    Debugger.Break();
                return false;
            }

            return true;
        }
    }
}