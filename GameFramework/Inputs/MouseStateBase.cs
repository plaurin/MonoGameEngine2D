using System;
using System.Collections.Generic;

using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public abstract class MouseStateBase
    {
        public abstract bool IsButtonDown(MouseButtons button);

        public virtual Vector AbsolutePosition
        {
            get
            {
                return this.Position;
            }
        }

        public abstract Vector Position { get; }

        public MouseStateBase AdjustToCamera(ICamera camera)
        {
            return new AdjustedMouseState(this, camera);
        }

        public override string ToString()
        {
            var buttonList = new List<string>();
            if (this.IsButtonDown(MouseButtons.Left)) buttonList.Add("Left");
            if (this.IsButtonDown(MouseButtons.Middle)) buttonList.Add("Middle");
            if (this.IsButtonDown(MouseButtons.Right)) buttonList.Add("Right");

            return string.Format("{0} {1}", this.Position, string.Join(", ", buttonList));
        }

        private class AdjustedMouseState : MouseStateBase
        {
            private readonly MouseStateBase inneMouseState;

            private readonly ICamera camera;

            public AdjustedMouseState(MouseStateBase inneMouseState, ICamera camera)
            {
                this.inneMouseState = inneMouseState;
                this.camera = camera;
            }

            public override bool IsButtonDown(MouseButtons button)
            {
                return this.inneMouseState.IsButtonDown(button);
            }

            public override Vector AbsolutePosition
            {
                get
                {
                    return this.inneMouseState.Position;
                }
            }

            public override Vector Position
            {
                get
                {
                    return this.inneMouseState.Position
                        .Translate(-this.camera.ViewPortCenter)
                        .Scale(1.0f / this.camera.ZoomFactor)
                        .Translate(this.camera.Position);
                }
            }
        }
    }
}