using KeyVaultBuild.Features.Transformation;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class TransformTests
    {
        [Test]
        public void TestRegex()
        {
            Assert.IsTrue(TransformKeys.IsKeySyntax("#{keyvault:keyvaultbuild-pre:test}"));
        }

        [Test]
        public void TestRegexNoDash()
        {
            Assert.IsTrue(TransformKeys.IsKeySyntax("#{keyvault:keyvaultbuild:test}"));
        }

        [Test]
        public void TestRegexNumbers()
        {
            Assert.IsTrue(TransformKeys.IsKeySyntax("#{keyvault:keyvaultbuild:test1230}"));
        }


        [Test]
        public void ReplaceInContentTest()
        {
            Assert.IsTrue(TransformKeys.IsKeySyntax("#{keyvault:keyvaultbuild:test}"));
        }
    }
}