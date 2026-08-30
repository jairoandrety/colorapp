using NUnit.Framework;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorApp.Tests
{
    public class ColorPaletteTests
    {
        private static ColorPalette PaletteWithThreeSlots()
        {
            var palette = new ColorPalette();
            palette.slots.Add(new ColorSlot("a", Color.red, Color.red));
            palette.slots.Add(new ColorSlot("b", Color.green, Color.green));
            palette.slots.Add(new ColorSlot("c", Color.blue, Color.blue));
            return palette;
        }

        [Test]
        public void GetSlot_WithIndexPastTheEnd_ReturnsTheLastSlot()
        {
            var palette = PaletteWithThreeSlots();

            ColorSlot slot = palette.GetSlot(999);

            Assert.AreEqual("c", slot.key);
        }

        [Test]
        public void GetSlot_WithNegativeIndex_ReturnsTheFirstSlot()
        {
            var palette = PaletteWithThreeSlots();

            ColorSlot slot = palette.GetSlot(-5);

            Assert.AreEqual("a", slot.key);
        }

        [Test]
        public void GetSlot_OnEmptyPalette_ReturnsNull()
        {
            var palette = new ColorPalette();

            Assert.IsNull(palette.GetSlot(0));
        }

        [Test]
        public void IndexOfKey_FindsTheMatchingSlot()
        {
            var palette = PaletteWithThreeSlots();

            Assert.AreEqual(1, palette.IndexOfKey("b"));
        }

        [Test]
        public void IndexOfKey_WithMissingKey_ReturnsMinusOne()
        {
            var palette = PaletteWithThreeSlots();

            Assert.AreEqual(-1, palette.IndexOfKey("nope"));
        }

        [Test]
        public void VariantName_FallsBackToEnumName_WhenBlank()
        {
            var palette = new ColorPalette { primaryVariantName = "", secondaryVariantName = null };

            Assert.AreEqual(ColorVariant.Primary.ToString(), palette.VariantName(ColorVariant.Primary));
            Assert.AreEqual(ColorVariant.Secondary.ToString(), palette.VariantName(ColorVariant.Secondary));
        }

        [Test]
        public void VariantName_UsesTheConfiguredName_WhenSet()
        {
            var palette = new ColorPalette { primaryVariantName = "Day", secondaryVariantName = "Night" };

            Assert.AreEqual("Day", palette.VariantName(ColorVariant.Primary));
            Assert.AreEqual("Night", palette.VariantName(ColorVariant.Secondary));
        }
    }
}
