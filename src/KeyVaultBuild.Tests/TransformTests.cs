﻿using KeyVaultBuild.Features.Transformation;
using NUnit.Framework;

namespace KeyVaultBuild.Tests
{
    [TestFixture]
    public class TransformTests
    {
        [Test]
        public void TestRegex()
        {
            Assert.IsTrue(TransformKey.IsKeySyntax("#{keyvault:keyvaultbuild:test}"));
        }

        [Test]
        public void ReplaceInContentTest()
        {
            Assert.IsTrue(TransformKey.IsKeySyntax("#{keyvault:keyvaultbuild:test}"));
        }
    }
}