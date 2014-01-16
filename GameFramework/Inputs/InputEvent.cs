using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Inputs
{
    public class InputEvent
    {
        private readonly List<TouchGestureType> mappingGestures;

        private Action<IGameTiming> touchAction;

        public InputEvent()
        {
            this.mappingGestures = new List<TouchGestureType>();
        }

        public InputEvent Assign(TouchGestureType gesture)
        {
            // Should it replace the assignment?
            this.mappingGestures.Add(gesture);

            return this;
        }

        public void MapTo(Action<IGameTiming> action)
        {
            this.touchAction = action;
        }

        public void Update(TouchStateBase touchState, IGameTiming gameTime)
        {
            if (this.touchAction != null && this.mappingGestures.Any(g => (g & touchState.CurrentGesture.GestureType) == g))
                this.touchAction.Invoke(gameTime);
        }
    }

    public class TouchGesture
    {
        private static TouchGesture noneTouchGesture;

        public TouchGesture(TouchGestureType gestureType)
        {
            this.GestureType = gestureType;
        }

        public TouchGesture(TouchGestureType gestureType, Vector position, Vector position2, Vector delta, Vector delta2)
        {
            this.GestureType = gestureType;
            this.Position = position;
            this.Position2 = position2;
            this.Delta = delta;
            this.Delta2 = delta2;
        }

        public TouchGestureType GestureType { get; private set; }

        public Vector Position { get; set; }

        public Vector Position2 { get; set; }

        public Vector Delta { get; set; }

        public Vector Delta2 { get; set; }

        public static TouchGesture None
        {
            get { return noneTouchGesture ?? (noneTouchGesture = new TouchGesture(TouchGestureType.None)); }
        }
    }

    [Flags]
    public enum TouchGestureType
    {
        None = 0,
        Tap = 1,
        DragComplete = 2,
        Flick = 4,
        FreeDrag = 8,
        Hold = 16,
        HorizontalDrag = 32,
        Pinch = 64,
        PinchComplete = 128,
        DoubleTap = 256,
        VerticalDrag = 512,
    }
}