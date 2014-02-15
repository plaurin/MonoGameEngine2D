using System;
using System.Collections.Generic;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;

namespace GameFramework.Utilities
{
    public class DiagnosticHudConfiguration
    {
        private readonly List<LineConfiguration> customLines;

        public DiagnosticHudConfiguration(DiagnosticDisplayLocation location = DiagnosticDisplayLocation.Right)
        {
            this.customLines = new List<LineConfiguration>();

            this.DisplayLocation = location;
            this.DisplayState = DiagnosticViewState.Full;
        }

        public DiagnosticDisplayLocation DisplayLocation { get; set; }

        public DiagnosticViewState DisplayState { get; set; }

        public IEnumerable<LineConfiguration> CustomLines
        {
            get { return this.customLines; }
        }

        public void EnableCameraTracking(ICamera camera)
        {
            this.AddLine("ViewPort: {0}", () => camera.SceneViewport);
            this.AddLine("Translation: {0}", () => camera.SceneTranslationVector);
            this.AddLine("Position: {0}", () => camera.Position);
            this.AddLine("Zooming: {0:f1}", () => camera.ZoomFactor);
        }

        public void EnableMouseTracking(MouseTracking mouseTracking)
        {
            MouseStateBase mouseState = null;
            mouseTracking.OnMove((mt, gt) => mouseState = mt);

            this.AddLine("Mouse: {0}", () => mouseState);
            this.AddLine("MouseAbs: {0}", () => mouseState.AbsolutePosition);
        }

        public void EnableTouchTracking(TouchTracking touchTracking)
        {
            TouchStateBase touchState = null;
            touchTracking.OnTouch((ts, gt) => touchState = ts);

            this.AddLine("TouchCap: Connected?: {0}, Pressure?: {1}",
                () => touchState.IsConnected, () => touchState.HasPressure);

            this.AddLine("TouchCap: MaxTouchCount: {0}, Gesture?: {1}",
                () => touchState.MaximumTouchCount, () => touchState.IsGestureAvailable);

            this.AddLine("Touches: {0}", () => string.Join("; ", touchState.Touches));
            this.AddLine("Gestures: {0}", () => touchState.CurrentGesture.GestureType);
        }

        public void EnableHitTracking(Func<IEnumerable<HitBase>> hitProviderFunc)
        {
            this.AddLine("Hits: {0}", () =>
            {
                var hits = hitProviderFunc();
                return hits != null ? string.Join("; ", hits) : null;
            });
        }

        public void AddLine(string template, params Func<object>[] parameterProviderFuncs)
        {
            this.customLines.Add(new LineConfiguration { Template = template, ParameterProviders = parameterProviderFuncs });
        }
    }

    public struct LineConfiguration
    {
        public string Template { get; set; }

        public IEnumerable<Func<object>> ParameterProviders { get; set; }
    }
}