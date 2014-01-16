using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Inputs
{
    public class VisualButton
    {
        private readonly List<TouchGestureType> mappingGestures;
        private readonly List<MouseButtons> mappingMouseButtons;

        private Action<IGameTiming> touchAction;
        private Action<IGameTiming> hoverAction;
        private Action<IGameTiming> clickAction;

        public VisualButton(Rectangle rectangle)
        {
            this.Rectangle = rectangle;

            this.mappingGestures = new List<TouchGestureType>();
            this.mappingMouseButtons = new List<MouseButtons>();
        }

        public Rectangle Rectangle { get; private set; }

        public VisualButton Assign(TouchGestureType gesture)
        {
            // Should it replace the assignment?
            this.mappingGestures.Add(gesture);

            return this;
        }

        public VisualButton Assign(MouseButtons mouseButtons)
        {
            // Should it replace the assignment?
            this.mappingMouseButtons.Add(mouseButtons);

            return this;
        }

        public VisualButton MapTouchTo(Action<IGameTiming> action)
        {
            this.touchAction = action;

            return this;
        }

        public VisualButton MapHoverTo(Action<IGameTiming> action)
        {
            this.hoverAction = action;

            return this;
        }

        public VisualButton MapClickTo(Action<IGameTiming> action)
        {
            this.clickAction = action;

            return this;
        }

        public void Update(TouchStateBase touchState, MouseStateBase mouseState, IGameTiming gameTime)
        {
            if (this.touchAction != null &&
                touchState.Touches.Any(t => this.Rectangle.Intercept(t.Position)))
            {
                this.touchAction.Invoke(gameTime);
            }

            if (this.hoverAction != null &&
                this.Rectangle.Intercept(mouseState.Position))
            {
                this.hoverAction.Invoke(gameTime);
            }

            if (this.clickAction != null)
            {
                if (this.mappingGestures.Any(g => (g & touchState.CurrentGesture.GestureType) == g)
                    && this.Rectangle.Intercept(touchState.CurrentGesture.Position))
                {
                    this.clickAction.Invoke(gameTime);
                }

                if (this.mappingMouseButtons.Any(mouseState.IsButtonDown)
                    && this.Rectangle.Intercept(mouseState.Position))
                {
                    this.clickAction.Invoke(gameTime);
                }
            }
        }
    }
}