using System;
using System.Linq;
using GameFramework;
using GameFramework.Sprites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameFrameworkTests
{
    [TestClass]
    public class SpriteAnimationTemplateTests
    {
        [TestMethod]
        public void TestDefaultValueForEmptyAnimation()
        {
            var expected = new SpriteAnimation("tata", new SpriteAnimationFrame[] { });

            // ARRANGE
            var spriteSheet = new SpriteSheet(null, "toto");
            var template = spriteSheet.AddSpriteAnimationTemplate("tata");

            // ACT
            var actual = (SpriteAnimation)template.CreateInstance();

            // ASSERT
            AssertSprite.AnimationEqual(expected, actual);
        }

        [TestMethod]
        public void TestSimpleAnimation()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            // ARRANGE
            var spriteSheet = new SpriteSheet(null, "toto");
            var definition1 = spriteSheet.AddSpriteDefinition("toto1", source, origin);
            var definition2 = spriteSheet.AddSpriteDefinition("toto2", source, origin);
            var template = spriteSheet.AddSpriteAnimationTemplate("tata")
                .AddFrame(definition1, 1.0f)
                .AddFrame(definition2, 2.5f);

            var expectedSprite1 = new Sprite(spriteSheet, "toto1");
            var expectedSprite2 = new Sprite(spriteSheet, "toto2");
            var expected = new SpriteAnimation("tata", new[]
            {
                new SpriteAnimationFrame(expectedSprite1, 1.0f),
                new SpriteAnimationFrame(expectedSprite2, 2.5f)
            });

            // ACT
            var actual = (SpriteAnimation)template.CreateInstance();

            // ASSERT
            AssertSprite.AnimationEqual(expected, actual);
        }

        [TestMethod]
        public void TestAnimationWithTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);
            var transform1 = new SpriteTransform(translation: new Vector(4, -5), rotation: 2.4f, scale: 1.5f, color: Color.Blue);
            var transform2 = new SpriteTransform(translation: new Vector(-6, 3), rotation: -1.2f, scale: 3.0f, color: Color.Red);

            // ARRANGE
            var spriteSheet = new SpriteSheet(null, "toto");
            var definition1 = spriteSheet.AddSpriteDefinition("toto1", source, origin);
            var definition2 = spriteSheet.AddSpriteDefinition("toto2", source, origin);
            var template = spriteSheet.AddSpriteAnimationTemplate("tata")
                .AddFrame(definition1, 1.0f, transform1)
                .AddFrame(definition2, 2.5f, transform2);

            var expectedSprite1 = new Sprite(spriteSheet, "toto1");
            var expectedSprite2 = new Sprite(spriteSheet, "toto2");
            var expected = new SpriteAnimation("tata", new[]
            {
                new SpriteAnimationFrame(expectedSprite1, 1.0f, transform1),
                new SpriteAnimationFrame(expectedSprite2, 2.5f, transform2)
            });

            // ACT
            var actual = (SpriteAnimation)template.CreateInstance();

            // ASSERT
            AssertSprite.AnimationEqual(expected, actual);
        }
    }
}