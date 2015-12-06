using EnterpriseApps.Portable.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace EnterpriseApps.Portable.Tests
{
    [TestClass]
    public class ListExtensionsTests
    {
        [TestMethod]
        public void TestFilterWithLessItems()
        {
            // Arrange
            var collection = new List<string>()
            {
                "Philipp",
                "Carsten",
                "Petra"
            };
            var filteredCollection = new List<string>()
            {
                "Philipp"
            };

            // Act
            collection.Filter(filteredCollection);

            // Assert
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(filteredCollection.First(), collection.First());
        }

        [TestMethod]
        public void TestFilterWithMoreItems()
        {
            // Arrange
            var collection = new List<string>()
            {
                "Philipp"
            };
            var filteredCollection = new List<string>()
            {
                "Philipp",
                "Carsten",
                "Petra"
            };

            // Act
            collection.Filter(filteredCollection);

            // Assert
            Assert.AreEqual(3, collection.Count);
        }
    }
}
