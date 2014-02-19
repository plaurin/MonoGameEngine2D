using System;
using System.Runtime.InteropServices;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Sprites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameFrameworkTests
{
    [TestClass]
    public class SpriteTransformTests
    {
        private const float HalfPI = (float)Math.PI / 2.0f;
        private const float Epsilon = 0.00001f;

        #region Color in isolation

        [TestMethod]
        public void TestIdentityColor()
        {
            var transform = new SpriteTransform();

            var final = transform.GetFinal();

            Assert.AreEqual(255, final.Color.R);
            Assert.AreEqual(255, final.Color.G);
            Assert.AreEqual(255, final.Color.B);
            Assert.AreEqual(255, final.Color.A);
        }

        [TestMethod]
        public void TestColorIdentityComposition()
        {
            var transform = new SpriteTransform(new SpriteTransform(new SpriteTransform()));

            var final = transform.GetFinal();

            Assert.AreEqual(255, final.Color.R);
            Assert.AreEqual(255, final.Color.G);
            Assert.AreEqual(255, final.Color.B);
            Assert.AreEqual(255, final.Color.A);
        }

        [TestMethod]
        public void TestColorNoComposition()
        {
            var transform = new SpriteTransform(color: new Color(100, 120, 140, 160));

            var final = transform.GetFinal();

            Assert.AreEqual(100, final.Color.R);
            Assert.AreEqual(120, final.Color.G);
            Assert.AreEqual(140, final.Color.B);
            Assert.AreEqual(160, final.Color.A);
        }

        [TestMethod]
        public void TestColorWithSimpleComposition()
        {
            var transform = new SpriteTransform(color: new Color(128, 192, 64, 32));
            var transform2 = new SpriteTransform(transform, color: new Color(255, 255, 255, 255));

            var final = transform2.GetFinal();

            Assert.AreEqual(128, final.Color.R);
            Assert.AreEqual(192, final.Color.G);
            Assert.AreEqual(64, final.Color.B);
            Assert.AreEqual(32, final.Color.A);
        }

        [TestMethod]
        public void TestColorWithComplexComposition()
        {
            var transform = new SpriteTransform(color: new Color(128, 192, 64, 32));
            var transform2 = new SpriteTransform(transform, color: new Color(128, 64, 32, 128));

            var final = transform2.GetFinal();

            Assert.AreEqual(64, final.Color.R);
            Assert.AreEqual(48, final.Color.G);
            Assert.AreEqual(8, final.Color.B);
            Assert.AreEqual(16, final.Color.A);
        }

        #endregion

        #region Translation in isolation

        [TestMethod]
        public void TestIdentityTranslation()
        {
            var transform = new SpriteTransform();

            var final = transform.GetFinal();

            Assert.AreEqual(Vector.Zero, final.Translation);
        }

        [TestMethod]
        public void TestTranslationIdentityComposition()
        {
            var transform = new SpriteTransform(new SpriteTransform(new SpriteTransform()));

            var final = transform.GetFinal();

            Assert.AreEqual(Vector.Zero, final.Translation);
        }

        [TestMethod]
        public void TestTranslationNoComposition()
        {
            var transform = new SpriteTransform(translation: new Vector(123, 456));

            var final = transform.GetFinal();

            Assert.AreEqual(new Vector(123, 456), final.Translation);
        }

        [TestMethod]
        public void TestTranslationWithSimpleComposition()
        {
            var transform1 = new SpriteTransform(translation: new Vector(98, 76));
            var transform2 = new SpriteTransform(transform1);

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(98, 76), final.Translation);
        }

        [TestMethod]
        public void TestTranslationWithComplexComposition()
        {
            var transform1 = new SpriteTransform(translation: new Vector(98, 76));
            var transform2 = new SpriteTransform(transform1, new Vector(-20, -30));

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(78, 46), final.Translation);
        }

        #endregion

        #region Scale in isolation

        [TestMethod]
        public void TestIdentityScale()
        {
            var transform = new SpriteTransform();

            var final = transform.GetFinal();

            Assert.AreEqual(1.0f, final.Scale);
        }

        [TestMethod]
        public void TestScaleIdentityComposition()
        {
            var transform = new SpriteTransform(new SpriteTransform(new SpriteTransform()));

            var final = transform.GetFinal();

            Assert.AreEqual(1.0f, final.Scale);
        }

        [TestMethod]
        public void TestScaleNoComposition()
        {
            var transform = new SpriteTransform(scale: 6.6f);

            var final = transform.GetFinal();

            Assert.AreEqual(6.6f, final.Scale);
        }

        [TestMethod]
        public void TestScaleWithSimpleComposition()
        {
            var transform1 = new SpriteTransform(scale: 2.4f);
            var transform2 = new SpriteTransform(transform1);

            var final = transform2.GetFinal();

            Assert.AreEqual(2.4f, final.Scale);
        }

        [TestMethod]
        public void TestScaleWithComplexComposition()
        {
            var transform1 = new SpriteTransform(scale: 0.4f);
            var transform2 = new SpriteTransform(transform1, scale: 8.0f);

            var final = transform2.GetFinal();

            Assert.AreEqual(3.2f, final.Scale);
        }

        #endregion

        #region Rotate in isolation

        [TestMethod]
        public void TestIdentityRotate()
        {
            var transform = new SpriteTransform();

            var final = transform.GetFinal();

            Assert.AreEqual(0.0f, final.Rotation);
        }

        [TestMethod]
        public void TestRotateIdentityComposition()
        {
            var transform = new SpriteTransform(new SpriteTransform(new SpriteTransform()));

            var final = transform.GetFinal();

            Assert.AreEqual(0.0f, final.Rotation);
        }

        [TestMethod]
        public void TestRotateNoComposition()
        {
            var transform = new SpriteTransform(rotation: 3.6f);

            var final = transform.GetFinal();

            Assert.AreEqual(3.6f, final.Rotation);
        }

        [TestMethod]
        public void TestRotateWithSimpleComposition()
        {
            var transform1 = new SpriteTransform(rotation: 5.4f);
            var transform2 = new SpriteTransform(transform1);

            var final = transform2.GetFinal();

            Assert.AreEqual(5.4f, final.Rotation);
        }

        [TestMethod]
        public void TestRotateWithComplexComposition()
        {
            var transform1 = new SpriteTransform(rotation: 1.4f);
            var transform2 = new SpriteTransform(transform1, rotation: -5.0f);

            var final = transform2.GetFinal();

            Assert.AreEqual(-3.6f, final.Rotation);
        }

        #endregion

        #region Translate and Scaling

        [TestMethod]
        public void TestTranslationAfterScaling()
        {
            var transform1 = new SpriteTransform(scale: 2.0f);
            var transform2 = new SpriteTransform(transform1, new Vector(100, 100));

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(200, 200), final.Translation);
            Assert.AreEqual(2.0f, final.Scale);
        }

        [TestMethod]
        public void TestScalingAfterTranslation()
        {
            var transform1 = new SpriteTransform(translation: new Vector(100, 100));
            var transform2 = new SpriteTransform(transform1, scale: 2.0f);

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(100, 100), final.Translation);
            Assert.AreEqual(2.0f, final.Scale);
        }

        [TestMethod]
        public void TestTranslationAfterScalingAndTranslate()
        {
            var transform1 = new SpriteTransform(translation: new Vector(15, -25), scale: 2.0f);
            var transform2 = new SpriteTransform(transform1, new Vector(100, 100));

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(215, 175), final.Translation);
            Assert.AreEqual(2.0f, final.Scale);
        }

        [TestMethod]
        public void TestScaleAndTranslationAfterTranslate()
        {
            var transform1 = new SpriteTransform(translation: new Vector(100, 100));
            var transform2 = new SpriteTransform(transform1, new Vector(15, -25), scale: 2.0f);

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(115, 75), final.Translation);
            Assert.AreEqual(2.0f, final.Scale);
        }

        #endregion

        #region Translate and Rotation

        [TestMethod]
        public void TestTranslationAfterRotation()
        {
            var transform1 = new SpriteTransform(rotation: HalfPI);
            var transform2 = new SpriteTransform(transform1, new Vector(100, 0));

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(0, 100), final.Translation);
            Assert.AreEqual(HalfPI, final.Rotation);
        }

        [TestMethod]
        public void TestRotationAfterTranslation()
        {
            var transform1 = new SpriteTransform(translation: new Vector(100, 100));
            var transform2 = new SpriteTransform(transform1, rotation: -HalfPI);

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(100, 100), final.Translation);
            Assert.AreEqual(-HalfPI, final.Rotation);
        }

        [TestMethod]
        public void TestTranslationAfterRotationAndTranslate()
        {
            var transform1 = new SpriteTransform(translation: new Vector(100, 100), rotation: HalfPI * 2);
            var transform2 = new SpriteTransform(transform1, new Vector(50, 25));

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(50, 75), final.Translation);
            Assert.AreEqual(HalfPI * 2, final.Rotation);
        }

        [TestMethod]
        public void TestRotationAndTranslationAfterTranslate()
        {
            var transform1 = new SpriteTransform(translation: new Vector(100, 100));
            var transform2 = new SpriteTransform(transform1, new Vector(15, -25), -HalfPI);

            var final = transform2.GetFinal();

            Assert.AreEqual(new Vector(115, 75), final.Translation);
            Assert.AreEqual(-HalfPI, final.Rotation);
        }

        #endregion

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
            sprite.Draw(drawContext.Object, SpriteTransform.Identity);

            // ASSERT
            drawContext.VerifyAll();
            AssertDrawImageParamsAreEquals(expected, actual);
        }

        [TestMethod]
        public void TestSpriteDrawWithPositionAndIdentityTransform()
        {
            var source = new RectangleInt(10, 10, 20, 20);
            var origin = new Vector(4, 7);

            var expected = CreateDrawImageParams(source, origin, destination: new Rectangle(-12, 24, 20, 20));

            // ARRANGE
            var sprite = CreateSprite(source, origin);
            sprite.Position = new Vector(-12, 24);

            DrawImageParams actual = null;
            var drawContext = CreateDrawContextMock(p => actual = p);

            // ACT
            sprite.Draw(drawContext.Object, SpriteTransform.Identity);

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
            sprite.Draw(drawContext.Object, SpriteTransform.Identity);

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
            sprite.Draw(drawContext.Object, SpriteTransform.Identity);

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
            sprite.Draw(drawContext.Object, SpriteTransform.Identity);

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
            var transform = new SpriteTransform(translation: new Vector(3, 5), rotation: 7, scale: 0.5f, color: Color.Blue);

            var expected = CreateDrawImageParams(source, origin, 
                destination: new Rectangle(3, 5, 10, 10), rotation: 7, color: Color.Blue);

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
        public void MethodeATester()
        {
            // ARRANGE

            // ACT

            // ASSERT
        }


        private static Sprite CreateSprite(RectangleInt source, Vector origin)
        {
            var spriteSheet = new SpriteSheet(null, "toto");
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
