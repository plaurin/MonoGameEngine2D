using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Inputs
{
    public class InputEvent
    {
        private readonly List<TouchGesture> mappingGestures;

        private Action<IGameTiming> touchAction;

        public InputEvent()
        {
            this.mappingGestures = new List<TouchGesture>();
        }

        public InputEvent Assign(TouchGesture gesture)
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
            if (this.touchAction != null && this.mappingGestures.Any(g => (g & touchState.CurrentGestures) == g))
                this.touchAction.Invoke(gameTime);
        }
    }

    [Flags]
    public enum TouchGesture
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