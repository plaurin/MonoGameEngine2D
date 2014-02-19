using System;
using System.Linq;
using GameFramework.Sprites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameFrameworkTests
{
    public class AssertSprite
    {
        public static void CompositeEqual(SpriteComposite expected, SpriteComposite actual, string path = "")
        {
            BaseEqual(expected, actual, path);

            Assert.AreEqual(expected.Children.Count(), actual.Children.Count(), path + "Children count");

            var tuples = expected.Children.OfType<SpriteBase>()
                .Zip(actual.Children.OfType<SpriteBase>(),
                    (e, a) => new Tuple<SpriteBase, SpriteBase>(e, a));

            var index = 0;
            foreach (var tuple in tuples)
            {
                var childPath = "C" + index + " - ";
                Assert.AreEqual(tuple.Item1.GetType(), tuple.Item2.GetType(), childPath + "Child Type");

                ChildSpriteEqual(tuple.Item1, tuple.Item2, childPath);

                index++;
            }
        }

        public static void AnimationEqual(SpriteAnimation expected, SpriteAnimation actual, string path = "")
        {
            BaseEqual(expected, actual, path);

            Assert.AreEqual(expected.Children.Count(), actual.Children.Count(), path + "Frame count");

            Assert.AreEqual(expected.AnimationState, actual.AnimationState);
            Assert.AreEqual(expected.TotalAnimationTime, actual.TotalAnimationTime);

            var tuples = expected.Children.OfType<SpriteAnimationFrame>()
                .Zip(actual.Children.OfType<SpriteAnimationFrame>(),
                    (e, a) => new Tuple<SpriteAnimationFrame, SpriteAnimationFrame>(e, a));

            var index = 0;
            foreach (var tuple in tuples)
            {
                var childPath = "A" + index + " - ";

                Assert.AreEqual(tuple.Item1.Duration, tuple.Item2.Duration, childPath + "Duration");
                Assert.AreEqual(tuple.Item1.Transform, tuple.Item2.Transform, childPath + "Transform");
                Assert.AreEqual(tuple.Item1.FrameSprite.GetType(), tuple.Item2.FrameSprite.GetType(), childPath + "FrameSprite Type");

                ChildSpriteEqual(tuple.Item1.FrameSprite, tuple.Item2.FrameSprite, childPath);

                index++;
            }
        }

        public static void Equal(Sprite expected, Sprite actual, string path = "")
        {
            BaseEqual(expected, actual, path);

            Assert.AreEqual(expected.SpriteSheet, actual.SpriteSheet, path + "SpriteSheet");
            Assert.AreEqual(expected.Origin, actual.Origin, path + "Origin");
            Assert.AreEqual(expected.FlipHorizontally, actual.FlipHorizontally, path + "FlipHorizontally");
            Assert.AreEqual(expected.FlipVertically, actual.FlipVertically, path + "FlipVertically");
        }

        public static void BaseEqual(SpriteBase expected, SpriteBase actual, string path = "")
        {
            Assert.AreEqual(expected.SpriteName, actual.SpriteName, path + "SpriteName");
            Assert.AreEqual(expected.Position, actual.Position, path + "Position");
            Assert.AreEqual(expected.Rotation, actual.Rotation, path + "Rotation");
            Assert.AreEqual(expected.Scale, actual.Scale, path + "Scale");
            Assert.AreEqual(expected.Color, actual.Color, path + "Color");
            Assert.AreEqual(expected.IsVisible, actual.IsVisible, path + "IsVisible");
        }

        private static void ChildSpriteEqual(SpriteBase expected, SpriteBase actual, string childPath)
        {
            var expectedSprite = expected as Sprite;
            if (expectedSprite != null)
            {
                var actualSprite = actual as Sprite;
                Assert.IsNotNull(actualSprite, childPath + "Actual should also be a Sprite");

                Equal(expectedSprite, actualSprite, childPath);
            }

            var expectedSpriteComposite = expected as SpriteComposite;
            if (expectedSpriteComposite != null)
            {
                var actualSpriteComposite = actual as SpriteComposite;
                Assert.IsNotNull(actualSpriteComposite, childPath + "Actual should also be a SpriteComposite");

                CompositeEqual(expectedSpriteComposite, actualSpriteComposite, childPath);
            }

            var expectedSpriteAnimation = expected as SpriteAnimation;
            if (expectedSpriteAnimation != null)
            {
                var actualSpriteAnimation = actual as SpriteAnimation;
                Assert.IsNotNull(actualSpriteAnimation, childPath + "Actual should also be a SpriteAnimation");

                AnimationEqual(expectedSpriteAnimation, actualSpriteAnimation, childPath);
            }
        }
    }
}