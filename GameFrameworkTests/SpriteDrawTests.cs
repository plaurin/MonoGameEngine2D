using System;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Sprites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameFrameworkTests
{
    [TestClass]
    public class SpriteDrawTests
    {
        private const float HalfPI = (float)Math.PI / 2.0f;
        private const float Epsilon = 0.00001f;

        #region Sprite Draw with Identify Transform

        [TestMethod]
        public void TestSpriteDrawWithDefaultValuesAndIdentityTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            var expected = CreateDrawImageParams(source, origin, destination: new Rectangle(0, 0, 20, 20));

            // ARRANGE
            var sprite = CreateSprite(source, origin);

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, SpriteTransform.SpriteIdentity);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithPositionAndIdentityTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            var expected = CreateDrawImageParams(source, origin, destination: new Rectangle(29, 24, 20, 20));

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Position = new Vector(29, 24);

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, SpriteTransform.SpriteIdentity);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithRotationAndIdentityTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            var expected = CreateDrawImageParams(source, origin,
                destination: new Rectangle(0, 0, 20, 20), rotation: 12.345f);

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Rotation = 12.345f;

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, SpriteTransform.SpriteIdentity);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithScaleAndIdentityTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            var expected = CreateDrawImageParams(source, origin,
                destination: new Rectangle(0, 0, 60, 60));

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Scale = 3.0f;

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, SpriteTransform.SpriteIdentity);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithColorAndIdentityTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            var expected = CreateDrawImageParams(source, origin,
                destination: new Rectangle(0, 0, 20, 20), color: new Color(128, 233, 143, 145));

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Color = new Color(128, 233, 143, 145);

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, SpriteTransform.SpriteIdentity);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        #endregion

        #region Sprite Draw with Transform

        [TestMethod]
        public void TestSpriteDrawWithDefaultValuesAndTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);
            var transform = new SpriteTransform(translation: new Vector(3, 5), rotation: HalfPI, scale: 0.5f, color: Color.Blue);

            var expected = CreateDrawImageParams(source, origin,
                destination: new Rectangle(3, 5, 10, 10), rotation: HalfPI, color: Color.Blue);

            // ARRANGE
            var sprite = CreateSprite(source, origin);

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, transform);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithPositionAndTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);
            var transform = new SpriteTransform(translation: new Vector(11, 13));

            var expected = CreateDrawImageParams(source, origin, destination: new Rectangle(-1, 37, 20, 20));

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Position = new Vector(-12, 24);

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, transform);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithRotationAndTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);
            var transform = new SpriteTransform(rotation: -7.2f);

            var expected = CreateDrawImageParams(source, origin,
                destination: new Rectangle(0, 0, 20, 20), rotation: 14.343f);

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Rotation = 21.543f;

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, transform);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithScaleAndTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);
            var transform = new SpriteTransform(scale: 0.25f);

            var expected = CreateDrawImageParams(source, origin,
                destination: new Rectangle(0, 0, 40, 40));

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Scale = 8.0f;

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, transform);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithColorAndTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);
            var transform = new SpriteTransform(color: new Color(128, 128, 128, 128));

            var expected = CreateDrawImageParams(source, origin,
                destination: new Rectangle(0, 0, 20, 20), color: new Color(111, 122, 94, 72));

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Color = new Color(222, 244, 188, 144);

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, transform);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        #endregion

        [TestMethod]
        public void TestSpriteCompositeDrawWithDefaultValuesAndIdentityTransform()
        {
            var source1 = new RectangleInt(10, 10, 20, 20);
            var source2 = new RectangleInt(40, 40, 30, 30);
            var origin = new Vector(4, 7);

            var expected1 = CreateDrawImageParams(source1, origin, destination: new Rectangle(0, 0, 20, 20));
            var expected2 = CreateDrawImageParams(source2, origin, destination: new Rectangle(0, 0, 30, 30));

            // ARRANGE
            var sprite1 = CreateSprite(source1, origin);
            var sprite2 = CreateSprite(source2, origin);
            var spriteComposite = new SpriteComposite("tata", new[] { sprite1, sprite2 });

            DrawImageParams actual1 = null;
            DrawImageParams actual2 = null;
            var callCount = 0;
            var drawContext = CreateDrawContextMock(p =>
            {
                callCount++;
                if (callCount == 1) actual1 = p;
                if (callCount == 2) actual2 = p;
            });

            // ACT
            spriteComposite.Draw(drawContext.Object, SpriteTransform.SpriteIdentity);

            // ASSERT
            drawContext.VerifyAll();
            Assert.AreEqual(2, callCount);
            AssertDrawImageParamsAreEquals(expected1, actual1);
            AssertDrawImageParamsAreEquals(expected2, actual2);
        }

        [TestMethod]
        public void TestSpriteCompositeDrawWithChangesButIdentityTransform()
        {
            var source1 = new RectangleInt(10, 10, 20, 20);
            var source2 = new RectangleInt(40, 40, 30, 30);
            var origin = new Vector(4, 7);

            var expected1 = CreateDrawImageParams(source1, origin,
                destination: new Rectangle(12, 8, 10, 10), rotation: 4.2f, color: Color.Red);

            var expected2 = CreateDrawImageParams(source2, origin,
                destination: new Rectangle(12, 8, 15, 15), rotation: 4.2f, color: Color.Red);

            // ARRANGE
            var sprite1 = CreateSprite(source1, origin);
            var sprite2 = CreateSprite(source2, origin);
            var spriteComposite = new SpriteComposite("tata", new[] { sprite1, sprite2 })
            {
                Position = new Vector(12, 8),
                Rotation = 4.2f,
                Scale = 0.5f,
                Color = Color.Red
            };

            DrawImageParams actual1 = null;
            DrawImageParams actual2 = null;
            var callCount = 0;
            var drawContext = CreateDrawContextMock(p =>
            {
                callCount++;
                if (callCount == 1) actual1 = p;
                if (callCount == 2) actual2 = p;
            });

            // ACT
            spriteComposite.Draw(drawContext.Object, SpriteTransform.SpriteIdentity);

            // ASSERT
            drawContext.VerifyAll();
            Assert.AreEqual(2, callCount);
            AssertDrawImageParamsAreEquals(expected1, actual1);
            AssertDrawImageParamsAreEquals(expected2, actual2);
        }

        [TestMethod]
        public void TestSpriteCompositeDrawWithDefaultValuesAndTransform()
        {
            var source1 = new RectangleInt(10, 10, 20, 20);
            var source2 = new RectangleInt(40, 40, 30, 30);
            var origin = new Vector(4, 7);
            var transform = new SpriteTransform(translation: new Vector(-20, -10), rotation: -2.2f, scale: 5.0f, color: Color.Green);

            var expected1 = CreateDrawImageParams(source1, origin,
                destination: new Rectangle(-20, -10, 100, 100), rotation: -2.2f, color: Color.Green);

            var expected2 = CreateDrawImageParams(source2, origin,
                destination: new Rectangle(-20, -10, 150, 150), rotation: -2.2f, color: Color.Green);

            // ARRANGE
            var sprite1 = CreateSprite(source1, origin);
            var sprite2 = CreateSprite(source2, origin);
            var spriteComposite = new SpriteComposite("tata", new[] { sprite1, sprite2 });

            DrawImageParams actual1 = null;
            DrawImageParams actual2 = null;
            var callCount = 0;
            var drawContext = CreateDrawContextMock(p =>
            {
                callCount++;
                if (callCount == 1) actual1 = p;
                if (callCount == 2) actual2 = p;
            });

            // ACT
            spriteComposite.Draw(drawContext.Object, transform);

            // ASSERT
            drawContext.VerifyAll();
            Assert.AreEqual(2, callCount);
            AssertDrawImageParamsAreEquals(expected1, actual1);
            AssertDrawImageParamsAreEquals(expected2, actual2);
        }

        [TestMethod]
        //public void TestSpriteCompositeDrawWithChangesButTransform()
        //{
        //    var source1 = new RectangleInt(10, 10, 20, 20);
        //    var source2 = new RectangleInt(40, 40, 30, 30);
        //    var origin = new Vector(4, 7);
        //    var transform = new SpriteTransform(translation: new Vector(250, 150), rotation: HalfPI, scale: 5.0f, color: Color.Green);

        //    var expected1 = CreateDrawImageParams(source1, origin,
        //        destination: new Rectangle(12, 8, 50, 50), rotation: 4.2f, color: Color.Red);

        //    var expected2 = CreateDrawImageParams(source2, origin,
        //        destination: new Rectangle(12, 8, 75, 75), rotation: 4.2f, color: Color.Red);

        //    // ARRANGE
        //    var sprite1 = CreateSprite(source1, origin);
        //    var sprite2 = CreateSprite(source2, origin);
        //    var spriteComposite = new SpriteComposite("tata", new[] { sprite1, sprite2 })
        //    {
        //        Position = new Vector(12, 8),
        //        Rotation = HalfPI+ 12,
        //        Scale = 0.5f,
        //        Color = Color.Red
        //    };

        //    DrawImageParams actual1 = null;
        //    DrawImageParams actual2 = null;
        //    var callCount = 0;
        //    var drawContext = CreateDrawContextMock(p =>
        //    {
        //        callCount++;
        //        if (callCount == 1) actual1 = p;
        //        if (callCount == 2) actual2 = p;
        //    });

        //    // ACT
        //    spriteComposite.Draw(drawContext.Object, transform);

        //    // ASSERT
        //    drawContext.VerifyAll();
        //    Assert.AreEqual(2, callCount, "# of calls to IDrawContext.DrawImage");
        //    AssertDrawImageParamsAreEquals(expected1, actual1);
        //    AssertDrawImageParamsAreEquals(expected2, actual2);
        //}

        private static Sprite CreateSprite(RectangleInt source, Vector origin)
        {
            var spriteSheet = new SpriteSheet(null, "toto");
            return CreateSprite(spriteSheet, source, origin);
        }

        private static Sprite CreateSprite(SpriteSheet spriteSheet, RectangleInt source, Vector origin)
        {
            spriteSheet.AddSpriteDefinition("toto", source, origin);
            var sprite = new Sprite(spriteSheet, "toto");

            return sprite;
        }

        private static Mock<IDrawContext> CreateDrawContextMock(Action<DrawImageParams> drawImageAction)
        {
            var drawContext = new Mock<IDrawContext>();
            drawContext.Setup(x => x.Camera).Returns(new Camera(new Viewport(0, 0, 1000, 1000)));
            drawContext.Setup(x => x.DrawImage(It.IsAny<DrawImageParams>())).Callback(drawImageAction);

            return drawContext;
        }

        private static DrawImageParams CreateDrawImageParams(RectangleInt source, Vector origin,
            Texture texture = null, Rectangle? destination = null, float rotation = 0.0f, Color? color = null)
        {
            var expected = new DrawImageParams
            {
                Texture = texture,
                Source = source,
                Destination = destination.HasValue ? destination.Value : Rectangle.Empty,
                Rotation = rotation,
                Origin = origin,
                Color = color.HasValue ? color.Value : Color.White,
                ImageEffect = ImageEffect.None
            };
            return expected;
        }

        private static void AssertDrawImageParamsAreEquals(DrawImageParams expected, DrawImageParams actual)
        {
            Assert.AreEqual(expected.Texture, actual.Texture, "Texture");
            Assert.AreEqual(expected.Source, actual.Source, "Source");
            Assert.AreEqual(expected.Destination, actual.Destination, "Destination");
            Assert.AreEqual(expected.Rotation, actual.Rotation, Epsilon, "Rotation");
            Assert.AreEqual(expected.Origin, actual.Origin, "Origin");
            Assert.AreEqual(expected.Color, actual.Color, "Color");
            Assert.AreEqual(expected.ImageEffect, actual.ImageEffect, "ImageEffect");
        }
    }
}