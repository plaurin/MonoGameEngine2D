using System;
using GameFramework;
using GameFramework.Sprites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameFrameworkTests
{
    [TestClass]
    public class SpriteTransformTests
    {
        private const float HalfPI = (float)Math.PI / 2.0f;

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
    }
}
