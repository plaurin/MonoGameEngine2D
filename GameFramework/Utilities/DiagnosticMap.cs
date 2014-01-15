using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Inputs;
using GameFramework.Maps;
using GameFramework.Scenes;

namespace GameFramework.Utilities
{
    public class DiagnosticMap : MapBase
    {
        private const int FirstLineY = 10;
        private const int LineHeight = 15;
        private const int LeftMargin = 10;
        private const int RightMargin = 350;

        private readonly DrawingMap map;
        private readonly DrawingFont font;
        private readonly DiagnosticMapConfiguration configuration;

        private readonly List<KeyValuePair<string, TextElement>> allLines;
        private readonly Dictionary<string, TextElement> customLines;

        private enum LineId
        {
            Fps,
            ViewPort,
            Translation,
            Position,
            Zoom,
            Mouse,
            MouseAbsolute,
            TouchCapabilities,
            TouchCapabilities2,
            Touches,
            Hits
        }

        public DiagnosticMap(GameResourceManager gameResourceManager, DrawingFont font, DiagnosticMapConfiguration configuration = null)
            : base("Diagnostic")
        {
            this.font = font;
            this.map = new DrawingMap("DiagnosticsInner", gameResourceManager) { CameraMode = CameraMode.Fix };
            this.configuration = configuration ?? new DiagnosticMapConfiguration();

            this.allLines = new List<KeyValuePair<string, TextElement>>();
            this.customLines = new Dictionary<string, TextElement>();

            // Create default diagnostic lines based on configuration
            if (this.configuration.DisplayFps)
                this.CreateNewLine(LineId.Fps, "FPS {0:d} - Update Per Second {1:d}");

            if (this.configuration.DisplayCameraState)
            {
                this.CreateNewLine(LineId.ViewPort, "ViewPort: {0}");
                this.CreateNewLine(LineId.Translation, "Translation: {0}");
                this.CreateNewLine(LineId.Position, "Position: {0}");
                this.CreateNewLine(LineId.Zoom, "Zooming: {0:f1}");
            }

            if (this.configuration.DisplayMouseState)
            {
                this.CreateNewLine(LineId.Mouse, "Mouse: {0}");
                this.CreateNewLine(LineId.MouseAbsolute, "MouseAbs: {0}");
            }

            if (this.configuration.DisplayTouchState)
            {
                this.CreateNewLine(LineId.TouchCapabilities, "TouchCap: Connected?: {0}, Pressure?: {1}");
                this.CreateNewLine(LineId.TouchCapabilities2, "TouchCap: MaxTouchCount: {0}, Gesture?: {1}");
                this.CreateNewLine(LineId.Touches, "Touches: {0}");
            }

            if (this.configuration.DisplayHits)
                this.CreateNewLine(LineId.Hits, "Hits: {0}");
        }

        public void Update(IGameTiming gameTime, Camera camera)
        {
            this.AdjustLinesPosition(camera);

            if (this.configuration.DisplayFps)
                this.UpdatBuiltInLine(LineId.Fps, gameTime.DrawFps, gameTime.UpdateFps);

            if (this.configuration.DisplayCameraState)
            {
                this.UpdatBuiltInLine(LineId.ViewPort, camera.SceneViewPort);
                this.UpdatBuiltInLine(LineId.Translation, camera.SceneTranslationVector);
                this.UpdatBuiltInLine(LineId.Position, camera.Position);
                this.UpdatBuiltInLine(LineId.Zoom, camera.ZoomFactor);
            }
        }

        public void Update(MouseStateBase mouseState)
        {
            if (this.configuration.DisplayMouseState && mouseState != null)
            {
                this.UpdatBuiltInLine(LineId.Mouse, mouseState);
                this.UpdatBuiltInLine(LineId.MouseAbsolute, mouseState.AbsolutePosition);
            }
        }

        public void Update(TouchStateBase touchState)
        {
            if (this.configuration.DisplayTouchState && touchState != null)
            {
                this.UpdatBuiltInLine(LineId.TouchCapabilities, touchState.IsConnected, touchState.HasPressure);
                this.UpdatBuiltInLine(LineId.TouchCapabilities2, touchState.MaximumTouchCount, touchState.IsGestureAvailable);
                this.UpdatBuiltInLine(LineId.Touches, string.Join("; ", touchState.Touches));
            }
        }

        public void Update(IEnumerable<HitBase> hits)
        {
            if (this.configuration.DisplayHits && hits != null)
                this.UpdatBuiltInLine(LineId.Hits, string.Join("; ", hits));
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            this.map.Draw(drawContext, camera);
        }

        public void AddLine(string lineId, string textFormat)
        {
            this.customLines.Add(lineId, this.CreateNewLine(null, textFormat));
        }

        public void UpdateLine(string lineId, params object[] parameters)
        {
            var textElement = this.customLines[lineId];
            textElement.SetParameters(parameters);
        }

        private void CreateNewLine(LineId lineId, string text)
        {
            this.CreateNewLine(lineId.ToString(), text);
        }

        private TextElement CreateNewLine(string lineId, string text)
        {
            var textElement = this.map.AddText(this.font, text, Vector.Zero, Color.White);
            this.allLines.Add(new KeyValuePair<string, TextElement>(lineId, textElement));
            return textElement;
        }

        private void UpdatBuiltInLine(LineId lineId, params object[] parameters)
        {
            this.allLines.Single(l => l.Key == lineId.ToString()).Value.SetParameters(parameters);
        }

        private void AdjustLinesPosition(Camera camera)
        {
            var currentY = FirstLineY;
            foreach (var textElement in this.allLines.Select(l => l.Value))
            {
                var x = this.configuration.DisplayLocation == DiagnosticDisplayLocation.Left
                    ? LeftMargin
                    : camera.Viewport.Width - RightMargin;

                textElement.Position = new Vector(x, currentY);
                currentY += LineHeight;
            }
        }
    }

    public class DiagnosticMapConfiguration
    {
        public DiagnosticMapConfiguration()
        {
            this.DisplayLocation = DiagnosticDisplayLocation.Right;
            this.DisplayFps = true;
            this.DisplayCameraState = true;
            this.DisplayMouseState = true;
            this.DisplayTouchState = true;
            this.DisplayHits = true;
        }

        public DiagnosticDisplayLocation DisplayLocation { get; set; }

        public bool DisplayFps { get; set; }

        public bool DisplayCameraState { get; set; }

        public bool DisplayMouseState { get; set; }

        public bool DisplayTouchState { get; set; }

        public bool DisplayHits { get; set; }

        public static DiagnosticMapConfiguration CreateWithFpsOnly(DiagnosticDisplayLocation location = DiagnosticDisplayLocation.Right)
        {
            return new DiagnosticMapConfiguration
            {
                DisplayLocation = location,
                DisplayCameraState = false,
                DisplayMouseState = false,
                DisplayTouchState = false,
                DisplayHits = false,
            };
        }
    }

    public enum DiagnosticDisplayLocation
    {
        Left,
        Right
    }
}