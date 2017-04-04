using KeyVaultBuild.Features.Authentication;
using KeyVaultBuild.Features.Config;
using KeyVaultBuild.Features.Operations;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void GetInteractiveToken()
        {
            var config = new Configuration { Directory = "773ff5d6-d53c-4063-baa2-8e542336ee29" };
            var token = new InteractiveAuthToken(config);
            var client = new AuthedClient(token);

            var reader = new ReadKey(client, "keyvaultbuild", "test");
            var result = reader.ExecuteAsync().Result;

            Assert.AreEqual("test", result);
        }

        [Test]
        public void SecretServiceTest()
        {
            var config = new Configuration { Directory = "773ff5d6-d53c-4063-baa2-8e542336ee29" };
            var s = new SecretService(config);
            var reader = s.ResolveSingleKey("#{keyvault:keyvaultbuild:test}");

            var result = reader.ExecuteAsync().Result;

            Assert.AreEqual("test", result);
        }
    }
}
