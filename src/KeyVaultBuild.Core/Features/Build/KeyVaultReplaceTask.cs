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

        public string OverrideVaults { get; set; }

        public string DebugTask { get; set; }

        public override bool Execute()
        {
            var debug = string.Equals("true", DebugTask, StringComparison.OrdinalIgnoreCase);
            Config.Log.Information = x => Log.LogMessage(x);
            Config.Log.Error = (ex, m) => Log.LogError("Error occured processing secrets. " + Environment.NewLine + ex);

            try
            {
                if (debug && !Debugger.IsAttached)
                {
                    Debugger.Launch();
                    Debugger.Break();
                }

                var service = SecretServiceBuilder.Create()
                    .WithDirectory(DirectoryId)
                    .WithServicePrincipal(ClientId, Secret)
                    .Build();
                var transformKey = new TransformKeys(service);

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