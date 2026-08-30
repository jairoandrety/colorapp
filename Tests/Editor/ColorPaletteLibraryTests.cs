using NUnit.Framework;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorApp.Tests
{
    public class ColorPaletteLibraryTests
    {
        private ColorPaletteLibrary library;

        [SetUp]
        public void SetUp()
        {
            library = ScriptableObject.CreateInstance<ColorPaletteLibrary>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(library);
        }

        [Test]
        public void HasPalettes_IsFalse_WhenEmpty()
        {
            Assert.IsFalse(library.HasPalettes);
        }

        [Test]
        public void ClampPaletteIndex_ReturnsMinusOne_WhenEmpty()
        {
            Assert.AreEqual(-1, library.ClampPaletteIndex(0));
        }

        [Test]
        public void ClampPaletteIndex_ClampsToTheLastPalette_WhenTooHigh()
        {
            library.Palettes.Add(new ColorPalette());
            library.Palettes.Add(new ColorPalette());

            Assert.AreEqual(1, library.ClampPaletteIndex(999));
        }

        [Test]
        public void ClampPaletteIndex_ClampsToTheFirstPalette_WhenNegative()
        {
            library.Palettes.Add(new ColorPalette());
            library.Palettes.Add(new ColorPalette());

            Assert.AreEqual(0, library.ClampPaletteIndex(-3));
        }

        [Test]
        public void Resolve_ReturnsFallback_WhenLibraryIsEmpty()
        {
            Color result = library.Resolve(0, 0, ColorVariant.Primary, Color.magenta);

            Assert.AreEqual(Color.magenta, result);
        }

        [Test]
        public void Resolve_ClampsBothPaletteAndSlotIndex_InsteadOfThrowing()
        {
            var small = new ColorPalette();
            small.slots.Add(new ColorSlot("only", Color.red, Color.blue));
            library.Palettes.Add(small);

            // Ni la paleta 50 ni el slot 50 existen: no debe lanzar, y debe
            // resolver contra el ultimo valor disponible en cada dimension.
            Color result = library.Resolve(50, 50, ColorVariant.Secondary, Color.magenta);

            Assert.AreEqual(Color.blue, result);
        }

        [Test]
        public void Resolve_UsesTheRequestedVariant()
        {
            var palette = new ColorPalette();
            palette.slots.Add(new ColorSlot("main", Color.red, Color.blue));
            library.Palettes.Add(palette);

            Assert.AreEqual(Color.red, library.Resolve(0, 0, ColorVariant.Primary, Color.magenta));
            Assert.AreEqual(Color.blue, library.Resolve(0, 0, ColorVariant.Secondary, Color.magenta));
        }
    }
}
