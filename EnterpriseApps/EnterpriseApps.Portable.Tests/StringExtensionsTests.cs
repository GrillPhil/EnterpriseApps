using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnterpriseApps.Portable.ExtensionMethods;

namespace EnterpriseApps.Portable.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestFirstCharToUpperWithEmptyString()
        {
            // Arrange
            var input = String.Empty;

            // Act
            var output = input.FirstCharToUpper();
        }

        [TestMethod]
        public void TestFirstCharToUpperWithProperString()
        {
            // Arrange
            var input = "bill";

            // Act
            var output = input.FirstCharToUpper();

            // Assert
            Assert.AreEqual("Bill", output);
        }
    }
}
