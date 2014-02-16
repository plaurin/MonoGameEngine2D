using System.Collections.Generic;
using GameFramework.Cameras;

namespace GameFramework.Inputs
{
    public abstract class TouchStateBase
    {
        public TouchStateBase AdjustToCamera(ICamera camera)
        {
            return new AdjustedTouchState(this, camera);
        }

        private class AdjustedTouchState : TouchStateBase
        {
            private readonly TouchStateBase innerTouchState;
            private readonly ICamera camera;

            public AdjustedTouchState(TouchStateBase innerTouchState, ICamera camera)
            {
                this.innerTouchState = innerTouchState;
                this.camera = camera;
            }

            public override IEnumerable<TouchPoint> Touches
            {
                get { return this.innerTouchState.Touches; }
            }

            public override bool IsConnected
            {
                get { return this.innerTouchState.IsConnected; }
            }

            public override bool HasPressure
            {
                get { return this.innerTouchState.HasPressure; }
            }

            public override int MaximumTouchCount
            {
                get { return this.innerTouchState.MaximumTouchCount; }
            }

            public override bool IsGestureAvailable
            {
                get { return this.innerTouchState.IsGestureAvailable; }
            }

            public override TouchGesture CurrentGesture
            {
                get { return this.innerTouchState.CurrentGesture; }
            }
        }

        public abstract IEnumerable<TouchPoint> Touches { get; }

        public abstract bool IsConnected { get; }
        
        public abstract bool HasPressure { get; }
        
        public abstract int MaximumTouchCount { get; }
        
        public abstract bool IsGestureAvailable { get; }

        public abstract TouchGesture CurrentGesture { get; }
    }

    public enum TouchPointState
    {
        Invalid = 0,
        Moved = 1,
        Pressed = 2,
        Released = 3,
    }
}