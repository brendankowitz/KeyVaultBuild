using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CommandLine;

namespace KeyVaultBuild.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<TemplateArguments>(args)
                .MapResult(
                  RunAddAndReturnExitCode,
                  errs => 1);
        }

        private static int RunAddAndReturnExitCode(TemplateArguments opts)
        {
            Console.WriteLine("** KeyVaultBuild Template File Processor **");
            try
            {
                var service = SecretServiceBuilder.Create()
                    .WithDirectory(opts.DirectoryId)
                    .Build();
                var writer = service.GetWriter(opts.Vault);

                var loaded = XDocument.Load(opts.ConfigFile);
                var appSettings = loaded.Descendants("appSettings").Descendants("add")
                    .Where(x =>
                    {
                        foreach (var pattern in opts.AppSettings)
                        {
                            if (Regex.IsMatch((string)x.Attribute("key"), pattern))
                            {
                                return true;
                            }
                        }

                        return false;
                    });

                var connectionStrings = loaded.Descendants("connectionStrings").Descendants("add");

                foreach (var setting in appSettings)
                {
                    var keyName = "appsetting-" + ((string) setting.Attribute("key")).ToSlug();
                    var val = (string)setting.Attribute("value");

                    if (!string.IsNullOrEmpty(keyName) && !string.IsNullOrEmpty(val))
                    {
                        writer.Set(keyName, val).Wait();
                        setting.SetAttributeValue("value", $"#{{keyvault:{writer.Vault}:{keyName}}}");
                    }
                }

                foreach (var setting in connectionStrings)
                {
                    var keyName = "connectionstring-" + ((string)setting.Attribute("name")).ToSlug();
                    var val = (string)setting.Attribute("connectionString");

                    if (!string.IsNullOrEmpty(keyName) && !string.IsNullOrEmpty(val))
                    {
                        writer.Set(keyName, val).Wait();
                        setting.SetAttributeValue("connectionString", $"#{{keyvault:{writer.Vault}:{keyName}}}");
                    }
                }

                var file = new FileInfo(opts.ConfigFile);
                var templateFile = Path.Combine(file.DirectoryName, file.Name.Replace(file.Extension, ".keyvault.template"));
                loaded.Save(File.OpenWrite(templateFile));

                Console.WriteLine("Successfully created template file '{0}'.", templateFile);
                Console.WriteLine("When ready you can delete the original config file and use the build task to restore the values from KeyVault.");
                Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }
    }
}
