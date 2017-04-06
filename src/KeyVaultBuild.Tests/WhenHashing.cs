using KeyVaultBuild.Features.Transformation;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class WhenHashing
    {
        [Test]
        public void GivenASimpleValue_ThenTheResultIsAsExpected()
        {
            var result = TransformFileExtensions.Hash("hello");
            Assert.AreEqual("2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824", result);
        }

        [Test]
        public void GivenAFileVersion_ThenTheHashCanBeExtracted()
        {
            var result = TransformFileExtensions.HashVersion.Match("<!-- test comment HashVersion:2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824 -->")
                .Groups[1].Value;
            Assert.AreEqual("2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824", result);
        }
    }
}