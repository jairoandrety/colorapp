using NUnit.Framework;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorApp.Tests
{
    public class ColorizerDataTests
    {
        [Test]
        public void GetOverride_ReturnsPrimary_ForPrimaryVariant()
        {
            var data = new ColorizerData { overridePrimary = Color.yellow, overrideSecondary = Color.cyan };

            Assert.AreEqual(Color.yellow, data.GetOverride(ColorVariant.Primary));
        }

        [Test]
        public void GetOverride_ReturnsSecondary_ForSecondaryVariant()
        {
            var data = new ColorizerData { overridePrimary = Color.yellow, overrideSecondary = Color.cyan };

            Assert.AreEqual(Color.cyan, data.GetOverride(ColorVariant.Secondary));
        }
    }
}
