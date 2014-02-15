using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Drawing;

namespace GameFramework.Utilities
{
    public class DiagnosticHud : IUpdatable, IDrawable
    {
        private const int FirstLineY = 10;
        private const int LineHeight = 15;
        private const int LeftMargin = 10;
        private const int RightMargin = 350;

        private readonly DrawingFont font;
        private readonly DiagnosticHudConfiguration configuration;

        private readonly LineConfiguration fpsLine;
        private readonly List<string> finalLines;

        private IGameTiming currentGameTime;

        public DiagnosticHud(DrawingFont font, DiagnosticHudConfiguration configuration = null)
        {
            this.font = font;
            this.configuration = configuration ?? new DiagnosticHudConfiguration();

            this.finalLines = new List<string>();
            this.fpsLine = new LineConfiguration
            {
                Template = "FPS {0:d} - Update Per Second {1:d}",
                ParameterProviders = new List<Func<object>>
                {
                    () => this.currentGameTime.DrawFps, 
                    () => this.currentGameTime.UpdateFps
                }
            };

            this.ViewState = this.configuration.DisplayState;
        }

        public DiagnosticViewState ViewState { get; set; }

        public void Update(IGameTiming gameTiming)
        {
            this.currentGameTime = gameTiming;

            var finalList = new List<LineConfiguration>();
            if (this.ViewState == DiagnosticViewState.FpsOnly || this.ViewState == DiagnosticViewState.Full)
            {
                finalList.Add(this.fpsLine);

                if (this.ViewState == DiagnosticViewState.Full)
                    finalList.AddRange(this.configuration.CustomLines);
            }

            this.finalLines.Clear();
            foreach (var line in finalList)
            {
                var parameters = line.ParameterProviders.Select(parameterProvider => parameterProvider.Invoke()).ToArray();
                this.finalLines.Add(string.Format(line.Template, parameters));
            }
        }

        public int Draw(IDrawContext drawContext)
        {
            var x = this.configuration.DisplayLocation == DiagnosticDisplayLocation.Left
                ? LeftMargin
                : drawContext.Camera.Viewport.Width - RightMargin;

            var currentY = FirstLineY;

            foreach (var line in this.finalLines)
            {
                this.DrawLine(drawContext, new Vector(x, currentY), line);
                currentY += LineHeight;
            }

            return this.finalLines.Count;
        }

        public void ViewStateToggle()
        {
            this.ViewState++;
            if (this.ViewState > DiagnosticViewState.Hide) this.ViewState = DiagnosticViewState.Full;
        }

        private void DrawLine(IDrawContext drawContext, Vector position, string text)
        {
            drawContext.DrawString(new DrawStringParams
            {
                Text = text,
                Vector = position,
                DrawingFont = this.font,
                Color = Color.White,
                ZoomFactor = 1.0f
            });
        }
    }

    public enum DiagnosticDisplayLocation
    {
        Left,
        Right
    }

    public enum DiagnosticViewState
    {
        Full,
        FpsOnly,
        Hide
    }
}