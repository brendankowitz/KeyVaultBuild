using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using KeyVaultBuild.Features.Config;

namespace KeyVaultBuild.Features.Transformation
{
    public static class TransformFileExtensions
    {
        internal static Regex HashVersion = new Regex("HashVersion:([0-9A-F]+)", RegexOptions.Compiled | RegexOptions.Singleline);

        public static void SaveAsFile(this TransformKeys transform, string templatePath)
        {
            var info = new FileInfo(templatePath);
            var destinationPath = Path.Combine(info.DirectoryName, info.Name.Replace(".keyvault.template", ".config"));

            var content = File.ReadAllText(templatePath);
            var hash = Hash(content);

            if (File.Exists(destinationPath))
            {
                var destContent = File.ReadAllText(destinationPath);
                var match = HashVersion.Match(destContent);
                if (match.Success)
                {
                    var version = match.Groups[1].Value;
                    if (version.Trim() == hash)
                    {
                        Log.Information($"Skipping template '{templatePath}' output is already up-to-date.");
                        return;
                    }
                }
            }

            string transformedContent = transform.ReplaceKeys(content);
            transformedContent = transformedContent.Insert(transformedContent.IndexOf(Environment.NewLine) + 1, 
                $"<!-- Generated at {DateTime.Now}, edit the template as this file will be overridden, HashVersion:{hash} -->");
            File.WriteAllText(destinationPath, transformedContent);
        }

        public static string Hash(string content)
        {
            using (var hasher = new SHA256Managed())
            {
                var buff = hasher.ComputeHash(Encoding.UTF8.GetBytes(content));
                return BitConverter.ToString(buff).Replace("-", string.Empty);
            }
        }
    }
}