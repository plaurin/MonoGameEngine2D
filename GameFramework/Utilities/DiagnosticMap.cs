using System;
using System.Collections.Generic;
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

        private readonly TextElement fpsElement;
        private readonly TextElement viewPortElement;
        private readonly TextElement translationElement;
        private readonly TextElement positionElement;
        private readonly TextElement zoomingElement;
        private readonly TextElement mouseElement;
        private readonly TextElement mouseElement2;
        private readonly TextElement hitElement;

        private readonly List<TextElement> lines;
        private readonly Dictionary<string, TextElement> customLines;

        public DiagnosticMap(GameResourceManager gameResourceManager, DrawingFont font, DiagnosticMapConfiguration configuration = null)
            : base("Diagnostic")
        {
            this.font = font;
            this.map = new DrawingMap("DiagnosticsInner", gameResourceManager) { CameraMode = CameraMode.Fix };
            this.configuration = configuration ?? new DiagnosticMapConfiguration();

            this.lines = new List<TextElement>();
            this.customLines = new Dictionary<string, TextElement>();

            // Create default diagnostic lines based on configuration
            if (this.configuration.DisplayFps)
                this.fpsElement = this.CreateNewLine("FPS {0:d} - Update Per Second {1:d}");

            if (this.configuration.DisplayCameraState)
            {
                this.viewPortElement = this.CreateNewLine("ViewPort: {0}");
                this.translationElement = this.CreateNewLine("Translation: {0}");
                this.positionElement = this.CreateNewLine("Position: {0}");
                this.zoomingElement = this.CreateNewLine("Zooming: {0:f1}");
            }

            if (this.configuration.DisplayMouseState)
            {
                this.mouseElement = this.CreateNewLine("Mouse: {0}");
                this.mouseElement2 = this.CreateNewLine("MouseAbs: {0}");
            }

            if (this.configuration.DisplayHits)
                this.hitElement = this.CreateNewLine("Hits: {0}");
        }

        public void Update(IGameTiming gameTime, Camera camera, MouseStateBase mouseState = null, IEnumerable<HitBase> hits = null)
        {
            var currentY = FirstLineY;
            foreach (var textElement in this.lines)
            {
                var x = this.configuration.DisplayLocation == DiagnosticDisplayLocation.Left
                    ? LeftMargin
                    : camera.Viewport.Width - RightMargin;

                textElement.Position = new Vector(x, currentY);
                currentY += LineHeight;
            }

            if (this.configuration.DisplayFps)
                this.fpsElement.SetParameters(gameTime.DrawFps, gameTime.UpdateFps);

            if (this.configuration.DisplayCameraState)
            {
                this.viewPortElement.SetParameters(camera.SceneViewPort);
                this.translationElement.SetParameters(camera.SceneTranslationVector);
                this.positionElement.SetParameters(camera.Position);
                this.zoomingElement.SetParameters(camera.ZoomFactor);
            }

            if (this.configuration.DisplayMouseState && mouseState != null)
            {
                this.mouseElement.SetParameters(mouseState);
                this.mouseElement2.SetParameters(mouseState.AbsolutePosition);
            }

            if (this.configuration.DisplayHits && hits != null)
                this.hitElement.SetParameters(string.Join("; ", hits));
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            this.map.Draw(drawContext, camera);
        }

        public void AddLine(string lineId, string textFormat)
        {
            this.customLines.Add(lineId, this.CreateNewLine(textFormat));
        }

        public void UpdateLine(string lineId, params object[] parameters)
        {
            var textElement = this.customLines[lineId];
            textElement.SetParameters(parameters);
        }

        private TextElement CreateNewLine(string text)
        {
            var textElement = this.map.AddText(this.font, text, Vector.Zero, Color.White);
            this.lines.Add(textElement);
            return textElement;
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
            this.DisplayHits = true;
        }

        public DiagnosticDisplayLocation DisplayLocation { get; set; }

        public bool DisplayFps { get; set; }

        public bool DisplayCameraState { get; set; }

        public bool DisplayMouseState { get; set; }

        public bool DisplayHits { get; set; }

        public static DiagnosticMapConfiguration CreateWithFpsOnly(DiagnosticDisplayLocation location = DiagnosticDisplayLocation.Right)
        {
            return new DiagnosticMapConfiguration
            {
                DisplayLocation = location,
                DisplayCameraState = false,
                DisplayMouseState = false,
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