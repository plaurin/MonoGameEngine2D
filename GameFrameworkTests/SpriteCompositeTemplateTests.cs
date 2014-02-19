using System;
using System.Diagnostics;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Sprites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            AssertSpriteCompositeEqual(expected, actual);
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
            AssertSpriteCompositeEqual(expected, actual);
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
                        expectedSprite,
                    })
                })
            });

            // ACT
            var actual = (SpriteComposite)template.CreateInstance();

            // ASSERT
            AssertSpriteCompositeEqual(expected, actual);
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
            AssertSpriteCompositeEqual(expected, actual);
        }

        private static void AssertSpriteCompositeEqual(SpriteComposite expected, SpriteComposite actual, string path = "")
        {
            AssertSpriteBaseEqual(expected, actual, path);

            Assert.AreEqual(expected.Children.Count(), actual.Children.Count(), path + "Children count");

            var tuples = expected.Children.Zip(actual.Children, (e, a) => new Tuple<object, object>(e, a));

            var index = 0;
            foreach (var tuple in tuples)
            {
                var childPath = "C" + index + " - ";
                Assert.AreEqual(tuple.Item1.GetType(), tuple.Item2.GetType(), childPath + "Child Type");

                var expectedSprite = tuple.Item1 as Sprite;
                if (expectedSprite != null)
                {
                    var actualSprite = tuple.Item2 as Sprite;
                    Assert.IsNotNull(actualSprite, childPath + "Acutal should also be a Sprite");

                    AssertSpriteEqual(expectedSprite, actualSprite, childPath);
                }

                var expectedSpriteComposite = tuple.Item1 as SpriteComposite;
                if (expectedSpriteComposite != null)
                {
                    var actualSpriteComposite = tuple.Item2 as SpriteComposite;
                    Assert.IsNotNull(actualSpriteComposite, childPath + "Acutal should also be a SpriteComposite");

                    AssertSpriteCompositeEqual(expectedSpriteComposite, actualSpriteComposite, childPath);
                }

                index++;
            }
        }

        private static void AssertSpriteEqual(Sprite expected, Sprite actual, string path = "")
        {
            AssertSpriteBaseEqual(expected, actual, path);

            Assert.AreEqual(expected.SpriteSheet, actual.SpriteSheet, path + "SpriteSheet");
            Assert.AreEqual(expected.Origin, actual.Origin, path + "Origin");
            Assert.AreEqual(expected.FlipHorizontally, actual.FlipHorizontally, path + "FlipHorizontally");
            Assert.AreEqual(expected.FlipVertically, actual.FlipVertically, path + "FlipVertically");
        }

        private static void AssertSpriteBaseEqual(SpriteBase expected, SpriteBase actual, string path = "")
        {
            Assert.AreEqual(expected.SpriteName, actual.SpriteName, path + "SpriteName");
            Assert.AreEqual(expected.Position, actual.Position, path + "Position");
            Assert.AreEqual(expected.Rotation, actual.Rotation, path + "Rotation");
            Assert.AreEqual(expected.Scale, actual.Scale, path + "Scale");
            Assert.AreEqual(expected.Color, actual.Color, path + "Color");
            Assert.AreEqual(expected.IsVisible, actual.IsVisible, path + "IsVisible");
        }
    }
}