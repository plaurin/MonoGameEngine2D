using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.Inputs;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonoGameImplementation.EngineImplementation
{
    public class XnaInputContext : InputContext
    {
        public override KeyboardStateBase KeyboardGetState()
        {
            return new XnaKeyboardState(Microsoft.Xna.Framework.Input.Keyboard.GetState());
        }

        public override MouseStateBase MouseGetState()
        {
            return new XnaMouseState(Mouse.GetState());
        }

        public override TouchStateBase TouchGetState()
        {
            GestureSample? gestureSample = null;
            if (TouchPanel.IsGestureAvailable) gestureSample = TouchPanel.ReadGesture();

            return new XnaTouchState(TouchPanel.GetState(), TouchPanel.GetCapabilities(), TouchPanel.IsGestureAvailable,
                gestureSample);
        }

        public override void UpdateEnabledGestures(IEnumerable<TouchGestureType> enabledGestures)
        {
            TouchPanel.EnabledGestures = XnaInputContext.GetGestures(enabledGestures);
        }

        private static GestureType GetGestures(IEnumerable<TouchGestureType> touchGestures)
        {
            var result = GestureType.None;
            foreach (var touchGesture in touchGestures)
            {
                switch (touchGesture)
                {
                    case TouchGestureType.None: result |= GestureType.None;
                        break;
                    case TouchGestureType.Tap: result |= GestureType.Tap;
                        break;
                    case TouchGestureType.DragComplete: result |= GestureType.DragComplete;
                        break;
                    case TouchGestureType.Flick: result |= GestureType.Flick;
                        break;
                    case TouchGestureType.FreeDrag: result |= GestureType.FreeDrag;
                        break;
                    case TouchGestureType.Hold: result |= GestureType.Hold;
                        break;
                    case TouchGestureType.HorizontalDrag: result |= GestureType.HorizontalDrag;
                        break;
                    case TouchGestureType.Pinch: result |= GestureType.Pinch;
                        break;
                    case TouchGestureType.PinchComplete: result |= GestureType.PinchComplete;
                        break;
                    case TouchGestureType.DoubleTap: result |= GestureType.DoubleTap;
                        break;
                    case TouchGestureType.VerticalDrag: result |= GestureType.VerticalDrag;
                        break;
                    default: throw new NotSupportedException("Gesture not supported yet");
                }
            }

            return result;
        }

        private class XnaKeyboardState : KeyboardStateBase
        {
            private KeyboardState states;

            public XnaKeyboardState(KeyboardState states)
            {
                this.states = states;
            }

            public override bool IsKeyDown(KeyboardKeys key)
            {
                return this.states.IsKeyDown(ConvertToXnaKey(key));
            }

            public override IEnumerable<KeyboardKeys> PressedKeys()
            {
                return this.states.GetPressedKeys().Select(ConvertToKeyboardKey);
            }

            private static Keys ConvertToXnaKey(KeyboardKeys key)
            {
                return (Keys)(int)key;
            }

            private static KeyboardKeys ConvertToKeyboardKey(Keys key)
            {
                return (KeyboardKeys)(int)key;
            }
        }

        private class XnaMouseState : MouseStateBase
        {
            private readonly MouseState mouseState;

            public XnaMouseState(MouseState mouseState)
            {
                this.mouseState = mouseState;
            }

            public override bool IsButtonDown(MouseButtons button)
            {
                switch (button)
                {
                    case MouseButtons.Left:
                        return this.mouseState.LeftButton == ButtonState.Pressed;
                    case MouseButtons.Middle:
                        return this.mouseState.MiddleButton == ButtonState.Pressed;
                    case MouseButtons.Right:
                        return this.mouseState.RightButton == ButtonState.Pressed;
                    default: 
                        throw new NotSupportedException("Button not supported yet");
                }
            }

            public override Vector Position
            {
                get
                {
                    return new Vector(this.mouseState.X, this.mouseState.Y);
                }
            }
        }

        private class XnaTouchState : TouchStateBase
        {
            private readonly TouchCollection touchCollection;
            private readonly TouchPanelCapabilities capabilities;
            private readonly bool isGestureAvailable;
            private readonly GestureSample? gestureSample;

            public XnaTouchState(TouchCollection touchCollection, TouchPanelCapabilities capabilities, bool isGestureAvailable,
                GestureSample? gestureSample)
            {
                this.touchCollection = touchCollection;
                this.capabilities = capabilities;
                this.isGestureAvailable = isGestureAvailable;
                this.gestureSample = gestureSample;
            }

            public override IEnumerable<TouchPoint> Touches
            {
                get
                {
                    return this.touchCollection.Select(t => 
                        new TouchPoint(t.Id, new Vector(t.Position.X, t.Position.Y), t.Pressure, (TouchPointState)t.State));
                }
            }

            public override bool IsConnected
            {
                get { return this.capabilities.IsConnected; }
            }

            public override bool HasPressure
            {
                get { return this.capabilities.HasPressure; }
            }

            public override int MaximumTouchCount
            {
                get { return this.capabilities.MaximumTouchCount; }
            }

            public override bool IsGestureAvailable
            {
                get { return this.isGestureAvailable; }
            }

            public override TouchGesture CurrentGesture
            {
                get
                {
                    if (!this.gestureSample.HasValue) return TouchGesture.None;

                    var gesture = this.gestureSample.Value;
                    return new TouchGesture((TouchGestureType)gesture.GestureType, gesture.Position.ToVector(),
                        gesture.Position2.ToVector(), gesture.Delta.ToVector(), gesture.Delta2.ToVector());
                }
            }
        }
    }
}