using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.Inputs;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Point = GameFramework.Point;

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

        public static GestureType GetGestures(IEnumerable<TouchGesture> touchGestures)
        {
            var result = GestureType.None;
            foreach (var touchGesture in touchGestures)
            {
                switch (touchGesture)
                {
                    case TouchGesture.None: result |= GestureType.None;
                        break;
                    case TouchGesture.Tap: result |= GestureType.Tap;
                        break;
                    case TouchGesture.DragComplete: result |= GestureType.DragComplete;
                        break;
                    case TouchGesture.Flick: result |= GestureType.Flick;
                        break;
                    case TouchGesture.FreeDrag: result |= GestureType.FreeDrag;
                        break;
                    case TouchGesture.Hold: result |= GestureType.Hold;
                        break;
                    case TouchGesture.HorizontalDrag: result |= GestureType.HorizontalDrag;
                        break;
                    case TouchGesture.Pinch: result |= GestureType.Pinch;
                        break;
                    case TouchGesture.PinchComplete: result |= GestureType.PinchComplete;
                        break;
                    case TouchGesture.DoubleTap: result |= GestureType.DoubleTap;
                        break;
                    case TouchGesture.VerticalDrag: result |= GestureType.VerticalDrag;
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

            private static Keys ConvertToXnaKey(KeyboardKeys key)
            {
                switch (key)
                {
                    case KeyboardKeys.Left: return Keys.Left;
                    case KeyboardKeys.Right: return Keys.Right;
                    case KeyboardKeys.Up: return Keys.Up;
                    case KeyboardKeys.Down: return Keys.Down;

                    case KeyboardKeys.D0: return Keys.D0;
                    case KeyboardKeys.D1: return Keys.D1;
                    case KeyboardKeys.D2: return Keys.D2;
                    case KeyboardKeys.D3: return Keys.D3;
                    case KeyboardKeys.D4: return Keys.D4;
                    case KeyboardKeys.D5: return Keys.D5;
                    case KeyboardKeys.D6: return Keys.D6;
                    case KeyboardKeys.D7: return Keys.D7;
                    case KeyboardKeys.D8: return Keys.D8;
                    case KeyboardKeys.D9: return Keys.D9;

                    case KeyboardKeys.Q: return Keys.Q;
                    case KeyboardKeys.W: return Keys.W;
                    case KeyboardKeys.A: return Keys.A;
                    case KeyboardKeys.Z: return Keys.Z;
                    case KeyboardKeys.Enter: return Keys.Enter;
                    case KeyboardKeys.Back: return Keys.Back;
                    case KeyboardKeys.Space: return Keys.Space;
                    case KeyboardKeys.Escape: return Keys.Escape;
                    default: throw new NotSupportedException("Key not supported yet");
                }
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

            public override Point Position
            {
                get
                {
                    return new Point(this.mouseState.X, this.mouseState.Y);
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

            public override TouchGesture CurrentGestures
            {
                get
                {
                    if (!this.gestureSample.HasValue) return TouchGesture.None;

                    return (TouchGesture)this.gestureSample.Value.GestureType;
                }
            }
        }
    }
}