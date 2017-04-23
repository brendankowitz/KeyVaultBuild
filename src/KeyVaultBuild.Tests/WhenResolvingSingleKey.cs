using System;
using System.Collections.Generic;
using KeyVaultBuild.Features.Operations;
using Microsoft.Azure.KeyVault;
using NSubstitute;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class WhenResolvingSingleKey
    {
        [Test]
        public void GivenAValidInput_ThenKeyAndVaultAreParsed()
        {
            var keyVaultClient = Substitute.For<IKeyVaultClient>();
            var service = new SecretService(new AuthedClient(keyVaultClient));

            var readKey = service.ResolveSingleKey("#{keyvault:vault:key}");

            Assert.AreEqual("vault", readKey.Vault);
            Assert.AreEqual("key", readKey.Key);
        }

        [Test]
        public void GivenAVaultAlias_ThenVaultIsOverriden()
        {
            var keyVaultClient = Substitute.For<IKeyVaultClient>();
            var vaultAlias = new Dictionary<string, string>
            {
                ["vault"] = "vault-dev"
            };

            var service = new SecretService(new AuthedClient(keyVaultClient), vaultAlias);

            var readKey = service.ResolveSingleKey("#{keyvault:vault:key}");

            Assert.AreEqual("vault-dev", readKey.Vault);
        }

        [Test]
        public void GivenNoColon_ThenExceptionIsThrown()
        {

            Assert.Throws<Exception>(() =>
            {
                var keyVaultClient = Substitute.For<IKeyVaultClient>();
                var service = new SecretService(new AuthedClient(keyVaultClient));

                var readKey = service.ResolveSingleKey("#{junk}");
            });
        }

        [Test]
        public void GivenToomanyColon_ThenExceptionIsThrown()
        {
            Assert.Throws<Exception>(() =>
            {
                var keyVaultClient = Substitute.For<IKeyVaultClient>();
                var service = new SecretService(new AuthedClient(keyVaultClient));

                var readKey = service.ResolveSingleKey("#{abc:def:ghi:jkl}");
            });
        }
    }
}
