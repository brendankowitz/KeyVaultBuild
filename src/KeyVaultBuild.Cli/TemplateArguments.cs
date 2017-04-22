using System.Collections.Generic;
using CommandLine;

namespace KeyVaultBuild.Cli
{
    [Verb("template", HelpText = "Creates a new template from an existing config file.")]
    public class TemplateArguments
    {
        [Option('c', "configFile", Required = true, HelpText = "Existing config file to process.")]
        public string ConfigFile { get; set; }

        [Option('d', "aadDirectory", Required = true, HelpText = "AAD to authenticate against.")]
        public string DirectoryId { get; set; }

        [Option('v', "vault", Required = true, HelpText = "KeyVault instance to upload values to.")]
        public string Vault { get; set; }

        [Option('a', "appsettings", Separator = ',', Required = false, HelpText = "Which appsettings, comma separated, to push into keyvault. Regex formatted")]
        public IEnumerable<string> AppSettings { get; set; }
    }
}