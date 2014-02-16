using System;
using GameFramework.Cameras;

namespace GameFramework.Layers
{
    public interface ILayer : IDrawable, INavigatorMetadataProvider
    {
        string Name { get; set; }

        CameraMode CameraMode { get; set; }

        Vector ParallaxScrollingVector { get; set; }

        Vector Offset { get; set; }

        bool IsVisible { get; set; }

        int TotalElements { get; }

        int DrawnElementsLastFrame { get; }
    }
}