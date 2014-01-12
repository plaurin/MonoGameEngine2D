using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
        private const int RightMargin = 350;

        private readonly DrawingMap map;
        private readonly DrawingFont font;

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

        private int nextLineY;

        public DiagnosticMap(GameResourceManager gameResourceManager, DrawingFont font)
            : base("Diagnostic")
        {
            this.font = font;
            this.map = new DrawingMap("DiagnosticsInner", gameResourceManager) { CameraMode = CameraMode.Fix };

            this.fpsElement = this.map.AddText(this.font, "FPS {0:d}", Vector.Zero, Color.White);
            this.viewPortElement = this.map.AddText(this.font, "ViewPort: {0}", Vector.Zero, Color.White);
            this.translationElement = this.map.AddText(this.font, "Translation: {0}", Vector.Zero, Color.White);
            this.positionElement = this.map.AddText(this.font, "Position: {0}", Vector.Zero, Color.White);
            this.zoomingElement = this.map.AddText(this.font, "Zooming: {0:f1}", Vector.Zero, Color.White);
            this.mouseElement = this.map.AddText(this.font, "Mouse: {0}", Vector.Zero, Color.White);
            this.mouseElement2 = this.map.AddText(this.font, "MouseAbs: {0}", Vector.Zero, Color.White);
            this.hitElement = this.map.AddText(this.font, "Hits: {0}", Vector.Zero, Color.White);

            this.lines = new List<TextElement>
            {
                this.fpsElement, 
                this.viewPortElement, 
                this.translationElement, 
                this.positionElement, 
                this.zoomingElement, 
                this.mouseElement, 
                this.mouseElement2,
                this.hitElement
            };

            this.nextLineY = FirstLineY + this.lines.Count * LineHeight;
            this.customLines = new Dictionary<string, TextElement>();
        }

        public void Update(IGameTiming gameTime, Camera camera, MouseStateBase mouseState, IEnumerable<HitBase> hits = null)
        {
            var currentY = FirstLineY;
            foreach (var textElement in this.lines)
            {
                textElement.Position = new Vector(camera.Viewport.Width - RightMargin, currentY);
                currentY += LineHeight;
            }

            this.fpsElement.SetParameters(gameTime.Fps);
            this.viewPortElement.SetParameters(camera.SceneViewPort);
            this.translationElement.SetParameters(camera.SceneTranslationVector);
            this.positionElement.SetParameters(camera.Position);
            this.zoomingElement.SetParameters(camera.ZoomFactor);
            this.mouseElement.SetParameters(mouseState);
            this.mouseElement2.SetParameters(mouseState.AbsolutePosition);

            if (hits != null)
                this.hitElement.SetParameters(string.Join("; ", hits));
        }

        public override void Draw(DrawContext drawContext, Camera camera)
        {
            this.map.Draw(drawContext, camera);
        }

        public override XElement ToXml()
        {
            throw new NotImplementedException();
        }

        public void AddLine(string lineId, string textFormat)
        {
            var textElement = this.map.AddText(this.font, textFormat, new Vector(410, this.nextLineY), Color.White);

            this.lines.Add(textElement);

            this.customLines.Add(lineId, textElement);
            this.nextLineY += 20;
        }

        public void UpdateLine(string lineId, params object[] parameters)
        {
            var textElement = this.customLines[lineId];
            textElement.SetParameters(parameters);
        }
    }
}