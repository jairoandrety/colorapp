#pragma warning disable CS0618 // los tipos legacy estan marcados [Obsolete] a proposito
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Jairoandrety.ColorApp;
using Jairoandrety.ColorAppEditor;

namespace Jairoandrety.ColorApp.Tests
{
    public class ColorAppMigrationTests
    {
        private static ColorPaletteSetup LegacySetupWithOnePalette()
        {
            var setup = ScriptableObject.CreateInstance<ColorPaletteSetup>();
            var palette = new Palette { paletteName = "Normal" };
            palette.colors.Add(new ColorPallete { label = "PrimaryColor", color = Color.red });
            palette.colors.Add(new ColorPallete { label = "Background", color = Color.white });
            setup.palettes.Add(palette);
            return setup;
        }

        [Test]
        public void Migrate_PreservesEveryColor_AsBothVariants()
        {
            var setup = LegacySetupWithOnePalette();

            ColorPaletteLibrary library = ColorAppMigration.Migrate(new List<ColorPaletteSetup> { setup });

            Assert.AreEqual(1, library.PaletteCount);
            ColorPalette palette = library.Palettes[0];
            Assert.AreEqual(2, palette.SlotCount);

            ColorSlot primaryColor = palette.slots[palette.IndexOfKey("PrimaryColor")];
            Assert.AreEqual(Color.red, primaryColor.primary);
            Assert.AreEqual(Color.red, primaryColor.secondary);

            // allowDestroyingAssets: true porque Migrate() ya escribio 'library'
            // en disco via AssetDatabase.CreateAsset.
            Object.DestroyImmediate(setup, true);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(library));
        }

        [Test]
        public void Migrate_WithNoSetups_ReturnsNull()
        {
            Assert.IsNull(ColorAppMigration.Migrate(new List<ColorPaletteSetup>()));
        }

        [Test]
        public void CanPairAsVariants_IsTrue_WhenKeysMatchInOrder()
        {
            var a = new ColorPalette();
            a.slots.Add(new ColorSlot("bg", Color.white, Color.white));
            var b = new ColorPalette();
            b.slots.Add(new ColorSlot("bg", Color.black, Color.black));

            Assert.IsTrue(ColorAppMigration.CanPairAsVariants(a, b));
        }

        [Test]
        public void CanPairAsVariants_IsFalse_WhenKeysDiffer()
        {
            var a = new ColorPalette();
            a.slots.Add(new ColorSlot("bg", Color.white, Color.white));
            var b = new ColorPalette();
            b.slots.Add(new ColorSlot("other", Color.black, Color.black));

            Assert.IsFalse(ColorAppMigration.CanPairAsVariants(a, b));
        }

        [Test]
        public void CanPairAsVariants_IsFalse_WhenSlotCountsDiffer()
        {
            var a = new ColorPalette();
            a.slots.Add(new ColorSlot("bg", Color.white, Color.white));
            var b = new ColorPalette();
            b.slots.Add(new ColorSlot("bg", Color.black, Color.black));
            b.slots.Add(new ColorSlot("extra", Color.black, Color.black));

            Assert.IsFalse(ColorAppMigration.CanPairAsVariants(a, b));
        }

        [Test]
        public void PairAsVariants_MovesEachPaletteIntoItsOwnVariant()
        {
            var light = new ColorPalette { displayName = "Normal" };
            light.slots.Add(new ColorSlot("bg", Color.white, Color.white));

            var dark = new ColorPalette { displayName = "Dark" };
            dark.slots.Add(new ColorSlot("bg", Color.black, Color.black));

            ColorPalette merged = ColorAppMigration.PairAsVariants(light, dark, "Brand");

            Assert.AreEqual("Brand", merged.displayName);
            Assert.AreEqual("Normal", merged.primaryVariantName);
            Assert.AreEqual("Dark", merged.secondaryVariantName);
            Assert.AreEqual(Color.white, merged.slots[0].primary);
            Assert.AreEqual(Color.black, merged.slots[0].secondary);
        }
    }
}
