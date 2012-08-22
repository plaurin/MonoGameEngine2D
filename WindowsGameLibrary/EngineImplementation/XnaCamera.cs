using System;

using ClassLibrary;
using ClassLibrary.Cameras;

namespace WindowsGame1.EngineImplementation
{
    public class XnaCamera : Camera
    {
        public XnaCamera(Viewport viewport)
            : base(viewport)
        {
        }

        public static XnaCamera CreateCamera(Microsoft.Xna.Framework.Graphics.Viewport viewport)
        {
            return new XnaCamera(new Viewport(viewport.Width, viewport.Height));
        }
    }
}