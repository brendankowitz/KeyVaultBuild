using KeyVaultBuild.Core.Features.Transformation;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class TransformTests
    {
        [Test]
        public void TestRegex()
        {
            Assert.IsTrue(KeyTransform.IsKeySyntax("#{keyvault:keyvaultbuild:test}"));
        }
    }
}