using KeyVaultBuild.Features.Transformation;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class TransformFileTests
    {
        [Test]
        public void TransformFile()
        {
            var service = SecretServiceBuilder.Create()
                .WithDirectory("773ff5d6-d53c-4063-baa2-8e542336ee29")
                .Build();

            var keys = new TransformKeys(service);

            var snippet = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""key1"" value=""#{keyvault:keyvaultbuild:test}"" />
    <add key=""key2"" value=""#{keyvault:keyvaultbuild:test}"" />
  </appSettings>
</configuration>";

            var expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""key1"" value=""test"" />
    <add key=""key2"" value=""test"" />
  </appSettings>
</configuration>";

            var val = keys.ReplaceKeys(snippet);

            Assert.AreEqual(expected, val);
        }
    }
}