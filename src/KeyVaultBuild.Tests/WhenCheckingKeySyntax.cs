using KeyVaultBuild.Features.Transformation;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class WhenCheckingKeySyntax
    {
        [Test]
        public void GivenVaultWithADash()
        {
            Assert.IsTrue(TransformKeys.IsKeySyntax("#{keyvault:keyvaultbuild-pre:test}"));
        }

        [Test]
        public void GivenAVaultWithNoDash()
        {
            Assert.IsTrue(TransformKeys.IsKeySyntax("#{keyvault:keyvaultbuild:test}"));
        }

        [Test]
        public void GivenAKeyWithNumbers()
        {
            Assert.IsTrue(TransformKeys.IsKeySyntax("#{keyvault:keyvaultbuild:test1230}"));
        }
    }
}