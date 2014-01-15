using System.Collections.Generic;
using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public abstract class TouchStateBase
    {
        public TouchStateBase AdjustToCamera(Camera camera)
        {
            return new AdjustedTouchState(this, camera);
        }

        private class AdjustedTouchState : TouchStateBase
        {
            private readonly TouchStateBase innerTouchState;
            private readonly Camera camera;

            public AdjustedTouchState(TouchStateBase innerTouchState, Camera camera)
            {
                this.innerTouchState = innerTouchState;
                this.camera = camera;
            }

            public override IEnumerable<TouchPoint> Touches
            {
                get { return this.innerTouchState.Touches; }
            }
        }

        public abstract IEnumerable<TouchPoint> Touches { get; }
    }

    public class TouchPoint
    {
        public TouchPoint(int id, Vector position, float pressure, TouchPointState state)
        {
            this.Id = id;
            this.Position = position;
            this.Pressure = pressure;
            this.State = state;
        }

        public int Id { get; private set; }

        public Vector Position { get; private set; }

        public float Pressure { get; private set; }

        public TouchPointState State { get; private set; }

        public override string ToString()
        {
            return string.Format("Id:{0}, Position:{1}, Pressure:{2:f2}, State:{3}", this.Id, this.Position, this.Pressure, this.State);
        }
    }

    public enum TouchPointState
    {
        Invalid = 0,
        Moved = 1,
        Pressed = 2,
        Released = 3,
    }
}