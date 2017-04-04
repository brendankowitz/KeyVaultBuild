using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KeyVaultBuild.Features.Transformation
{
    public static class TransformFileExtensions
    {
        public static void SaveAsFile(this TransformKeys transform, string templatePath)
        {
            var info = new FileInfo(templatePath);
            var destinationPath = Path.Combine(info.DirectoryName, info.Name.Replace(".keyvault.template", ".config"));

            var content = File.ReadAllText(templatePath);
            var hash = Hash(content);

            if (File.Exists(destinationPath))
            {
                //TODO: check hash, does it need regenerating?
            }

            string transformedContent = transform.ReplaceKeys(content);
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