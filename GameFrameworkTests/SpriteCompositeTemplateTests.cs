using System;
using System.Linq;
using GameFramework;
using GameFramework.Sprites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameFrameworkTests
{
    [TestClass]
    public class SpriteCompositeTemplateTests
    {
        [TestMethod]
        public void TestDefaultValueForEmptyComposite()
        {
            var expected = new SpriteComposite("tata", Enumerable.Empty<SpriteBase>());

            // ARRANGE
            var spriteSheet = new SpriteSheet(null, "toto");
            var template = spriteSheet.AddSpriteCompositeTemplate("tata");

            // ACT
            var actual = (SpriteComposite)template.CreateInstance();

            // ASSERT
            AssertSprite.CompositeEqual(expected, actual);
        }

        [TestMethod]
        public void TestComposite()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            // ARRANGE
            var spriteSheet = new SpriteSheet(null, "toto");
            var definition = spriteSheet.AddSpriteDefinition("toto", source, origin);
            var template = spriteSheet.AddSpriteCompositeTemplate("tata")
                .AddTemplate(definition);

            var expectedSprite = new Sprite(spriteSheet, "toto");
            var expected = new SpriteComposite("tata", new[] { expectedSprite });

            // ACT
            var actual = (SpriteComposite)template.CreateInstance();

            // ASSERT
            AssertSprite.CompositeEqual(expected, actual);
        }

        [TestMethod]
        public void TestMultipleComposite()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            // ARRANGE
            var spriteSheet = new SpriteSheet(null, "toto");
            var definition = spriteSheet.AddSpriteDefinition("toto", source, origin);
            var template = spriteSheet.AddSpriteCompositeTemplate("tata")
                .AddTemplate(definition)
                .AddTemplate(spriteSheet.AddSpriteCompositeTemplate("sub-tata")
                    .AddTemplate(definition)
                    .AddTemplate(spriteSheet.AddSpriteCompositeTemplate("sub-sub-tata")
                        .AddTemplate(definition)));

            var expectedSprite = new Sprite(spriteSheet, "toto");
            var expected = new SpriteComposite("tata", new SpriteBase[]
            {
                expectedSprite,
                new SpriteComposite("sub-tata", new SpriteBase[]
                {
                    expectedSprite,
                    new SpriteComposite("sub-sub-tata", new[]
                    {
                        expectedSprite
                    })
                })
            });

            // ACT
            var actual = (SpriteComposite)template.CreateInstance();

            // ASSERT
            AssertSprite.CompositeEqual(expected, actual);
        }

        [TestMethod]
        public void TestCompositeWithTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            // ARRANGE
            var spriteSheet = new SpriteSheet(null, "toto");
            var definition = spriteSheet.AddSpriteDefinition("toto", source, origin);
            var transform = new SpriteTransform(translation: new Vector(43, -27), rotation: -1.2f, scale: 5.0f, color: Color.Yellow);
            var template = spriteSheet.AddSpriteCompositeTemplate("tata")
                .AddTemplate(definition, transform);

            var expectedSprite = new Sprite(spriteSheet, "toto")
            {
                Position = new Vector(43, -27),
                Rotation = -1.2f,
                Scale = 5.0f,
                Color = Color.Yellow
            };

            var expected = new SpriteComposite("tata", new[] { expectedSprite });

            // ACT
            var actual = (SpriteComposite)template.CreateInstance();

            // ASSERT
            AssertSprite.CompositeEqual(expected, actual);
        }
    }
}