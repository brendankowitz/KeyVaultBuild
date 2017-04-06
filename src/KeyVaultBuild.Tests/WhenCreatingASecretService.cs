using System;
using KeyVaultBuild.Features.Authentication;
using KeyVaultBuild.Features.Config;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class WhenCreatingASecretService
    {
        [Test]
        public void GivenADirectory_ThenInteractiveLoginShouldBeUsed()
        {
            var provider = SecretService.ResolveTokenProvider(new Configuration {Directory = Guid.NewGuid().ToString()});
            Assert.IsInstanceOf<InteractiveAuthToken>(provider);
        }

        [Test]
        public void GivenADClientId_ThenCredentialsShouldBeUsed()
        {
            var provider = SecretService.ResolveTokenProvider(new Configuration { ServicePrincipal = Guid.NewGuid().ToString(), ServicePrincipalSecret = Guid.NewGuid().ToString() });
            Assert.IsInstanceOf<ServicePrincipalAuthToken>(provider);
        }

        [Test]
        public void GivenASecretServiceBuilderWithNoAuthInformation_ThenAnExceptionIsThrown()
        {
            Assert.Throws<Exception>(() => SecretServiceBuilder.Create().Build());
        }
    }
}
