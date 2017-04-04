using KeyVaultBuild.Features.Transformation;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class HashTests
    {
        [Test]
        public void TestHashing()
        {
            var result = TransformFile.Hash("hello");

            Assert.AreEqual("2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824", result);
        }
    }
}