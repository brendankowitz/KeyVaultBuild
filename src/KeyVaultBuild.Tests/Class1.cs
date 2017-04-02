using KeyVaultBuild.Core.Features.Authentication;
using KeyVaultBuild.Core.Features.Config;
using KeyVaultBuild.Core.Features.Operations;
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
    }
}
