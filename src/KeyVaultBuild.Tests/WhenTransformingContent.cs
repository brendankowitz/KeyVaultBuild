using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KeyVaultBuild.Features.Operations;
using KeyVaultBuild.Features.Transformation;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Rest.Azure;
using NSubstitute;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class WhenTransformingContent
    {
        [Test]
        public void GivenAConfig_ThenTheMockValuesAreReplacedFromKeyVault()
        {
            var service = Substitute.For<ISecretService>();
            var client = Substitute.For<IKeyVaultClient>();

            client.GetSecretWithHttpMessagesAsync(Arg.Any<string>(), Arg.Is("test1"), Arg.Any<string>(), Arg.Any<Dictionary<string, List<string>>>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult( new AzureOperationResponse<SecretBundle> { Body = new SecretBundle("secret1") }));

            var authedClient = new AuthedClient(client);
            var reader = new ReadKey(authedClient, "keyvaultbuild", "test1");
            service.ResolveSingleKey(Arg.Is("#{keyvault:keyvaultbuild:test1}")).Returns(reader);

            var keys = new TransformKeys(service);

            var snippet = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""key1"" value=""#{keyvault:keyvaultbuild:test1}"" />
  </appSettings>
</configuration>";

            var expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <appSettings>
    <add key=""key1"" value=""secret1"" />
  </appSettings>
</configuration>";

            var val = keys.ReplaceKeys(snippet);

            Assert.AreEqual(expected, val);
        }
    }
}