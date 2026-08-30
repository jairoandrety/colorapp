using NUnit.Framework;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorApp.Tests
{
    public class ColorSlotTests
    {
        [Test]
        public void Get_ReturnsPrimary_ForPrimaryVariant()
        {
            var slot = new ColorSlot("main", Color.red, Color.blue);
            Assert.AreEqual(Color.red, slot.Get(ColorVariant.Primary));
        }

        [Test]
        public void Get_ReturnsSecondary_ForSecondaryVariant()
        {
            var slot = new ColorSlot("main", Color.red, Color.blue);
            Assert.AreEqual(Color.blue, slot.Get(ColorVariant.Secondary));
        }

        [Test]
        public void Set_WritesTheRequestedVariantOnly()
        {
            var slot = new ColorSlot("main", Color.red, Color.blue);

            slot.Set(ColorVariant.Secondary, Color.green);

            Assert.AreEqual(Color.red, slot.primary);
            Assert.AreEqual(Color.green, slot.secondary);
        }
    }
}
