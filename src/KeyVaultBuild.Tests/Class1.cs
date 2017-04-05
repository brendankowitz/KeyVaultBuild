using System;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void SecretServiceTest()
        {
            var service = SecretServiceBuilder.Create()
                .WithDirectory("773ff5d6-d53c-4063-baa2-8e542336ee29")
                .Build();

            var reader = service.ResolveSingleKey("#{keyvault:keyvaultbuild:test}");
            var result = reader.ExecuteAsync().Result;

            Assert.AreEqual("test", result);
        }

        [Test]
        public void NoAuthInformationThrowsException()
        {
            Assert.Throws<Exception>(() => SecretServiceBuilder.Create().Build());
        }
    }
}
