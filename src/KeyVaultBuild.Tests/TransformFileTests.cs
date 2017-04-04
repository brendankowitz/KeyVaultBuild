using System.IO;
using KeyVaultBuild.Features.Config;
using KeyVaultBuild.Features.Transformation;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class TransformFileTests
    {
        [Test]
        public void TransformFile()
        {
            var config = new Configuration { Directory = "773ff5d6-d53c-4063-baa2-8e542336ee29" };
            var service = new SecretService(config);

            var keys = new TransformKey(service);

            var snippet = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""key1"" value=""#{keyvault:keyvaultbuild:test}"" />
    <add key=""key2"" value=""#{keyvault:keyvaultbuild:test}"" />
  </appSettings>
</configuration>";

            var val = keys.ReplaceKeys(snippet);
        }
    }
}