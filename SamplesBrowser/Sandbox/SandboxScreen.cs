﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Hexes;
using GameFramework.Inputs;
using GameFramework.IO.Repositories;
using GameFramework.Layers;
using GameFramework.Scenes;
using GameFramework.Screens;
using GameFramework.Sprites;
using GameFramework.Tiles;
using GameFramework.Utilities;

namespace SamplesBrowser.Sandbox
{
    public class SandboxScreen : SceneBasedScreen
    {
        private readonly ScreenNavigation screenNavigation;

        private readonly Vector player;

        private float range;

        private MouseStateBase mouseState;

        private DiagnosticHud diagnosticHud;

        private List<HitBase> hits;

        private Sprite linkMouseFollow;
        private ILayer layer;

        public SandboxScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;

            this.player = new Vector(25, 25);
            this.range = 0.25f;
        }

        public override void Update(IGameTiming gameTime)
        {
            var colorLayer = this.Scene.Children.OfType<ColorLayer>().FirstOrDefault();
            if (colorLayer != null)
                colorLayer.Color = new Color(255, 0, 0, (int)(255 * Math.Min(this.range, 1.0f)));
        }

        protected override Camera CreateCamera(Viewport viewport)
        {
            return new Camera(viewport);
        }

        protected override InputConfiguration CreateInputConfiguration()
        {
            var inputConfiguration = new InputConfiguration();

            inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

            inputConfiguration.AddDigitalButton("DiagToggle").Assign(KeyboardKeys.F1).MapClickTo(gt => this.diagnosticHud.ViewStateToggle());

            inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left).MapTo(gt => this.Camera.Move(-60 * gt.ElapsedSeconds, 0));
            inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right).MapTo(gt => this.Camera.Move(60 * gt.ElapsedSeconds, 0));
            inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up).MapTo(gt => this.Camera.Move(0, -60 * gt.ElapsedSeconds));
            inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down).MapTo(gt => this.Camera.Move(0, 60 * gt.ElapsedSeconds));

            inputConfiguration.AddDigitalButton("ZoomIn").Assign(KeyboardKeys.A).MapTo(gt => this.Camera.ZoomFactor *= 1.2f * (1 + gt.ElapsedSeconds));
            inputConfiguration.AddDigitalButton("ZoomOut").Assign(KeyboardKeys.Z).MapTo(gt => this.Camera.ZoomFactor *= 1 / (1.2f * (1 + gt.ElapsedSeconds)));

            inputConfiguration.AddDigitalButton("RangeUp").Assign(KeyboardKeys.W).MapTo(gt => this.range *= 1.2f * (1 + gt.ElapsedSeconds));
            inputConfiguration.AddDigitalButton("RangeDown").Assign(KeyboardKeys.Q).MapTo(gt => this.range *= 1 / (1.2f * (1 + gt.ElapsedSeconds)));

            inputConfiguration.AddDigitalButton("RotateLeft").Assign(KeyboardKeys.X).MapTo(gt => this.linkMouseFollow.Rotation -= 4 * gt.ElapsedSeconds);
            inputConfiguration.AddDigitalButton("RotateRight").Assign(KeyboardKeys.C).MapTo(gt => this.linkMouseFollow.Rotation += 4 * gt.ElapsedSeconds);

            inputConfiguration.AddDigitalButton("Selection").Assign(MouseButtons.Left).MapTo(elapse =>
            {
                this.hits = this.Scene.GetHits(this.mouseState.AbsolutePosition, this.Camera).ToList();
            });

            inputConfiguration.AddMouseTracking(this.Camera).OnMove((mt, e) =>
            {
                this.mouseState = mt;

                this.linkMouseFollow.Position = mt.AbsolutePosition;
            });

            return inputConfiguration;
        }

        protected override Scene CreateScene()
        {
            this.ResourceManager.AddDrawingFont(@"Sandbox\SpriteFont1");

            var scene = new Scene("scene1");

            scene.Add(this.CreateImageLayer());
            scene.Add(this.CreateHexLayer());
            scene.Add(this.CreateTileLayer());
            scene.Add(this.CreateColorLayer());
            scene.Add(this.CreateHexLayerTestDistance());
            scene.Add(this.CreateSpriteLayer());
            scene.Add(this.CreateDiagnosticLayer());
            scene.Add(this.CreateMouseCursor());
            scene.Add(this.CreateMouseFollow());

            //this.scene.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            //this.TestSceneSaveLoad();
            //this.TestSheetsSaveLoad();

            return scene;
        }

        private void TestSceneSaveLoad()
        {
            var scene2 = XmlRepository.LoadFrom(this.ResourceManager,
                @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            foreach (var layer in this.Scene.Children.OfType<ILayer>())
            {
                var otherLayer = scene2.Children.OfType<ILayer>().Single(x => x.Name == layer.Name);

                var myXml = XmlRepository.ToXml(layer); // map.ToXml();
                var otherXml = XmlRepository.ToXml(otherLayer); // otherMap.ToXml();

                if (myXml.ToString() != otherXml.ToString())
                {
                    //Debugger.Break();
                }
            }
        }

        private void TestSheetsSaveLoad()
        {
            var otherHexSheet = SheetXmlRepository.LoadFrom<HexSheet>(
                this.ResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color HexSheet.xml");

            var myHexSheetXml = SheetXmlRepository.ToXml(this.ResourceManager.GetHexSheet(otherHexSheet.Name));
            var otherHexSheetXml = SheetXmlRepository.ToXml(otherHexSheet);
            if (myHexSheetXml.ToString() != otherHexSheetXml.ToString()) Debugger.Break();

            var otherTileSheet = SheetXmlRepository.LoadFrom<TileSheet>(
                this.ResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color TileSheet.xml");

            var myTileSheetXml = SheetXmlRepository.ToXml(this.ResourceManager.GetTileSheet(otherTileSheet.Name));
            var otherTileSheetXml = SheetXmlRepository.ToXml(otherTileSheet);
            if (myTileSheetXml.ToString() != otherTileSheetXml.ToString()) Debugger.Break();

            var otherSpriteSheet = SheetXmlRepository.LoadFrom<SpriteSheet>(
                this.ResourceManager, @"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Link SpriteSheet.xml");

            var mySpriteSheetXml = SheetXmlRepository.ToXml(this.ResourceManager.GetSpriteSheet(otherSpriteSheet.Name));
            var otherSpriteSheetXml = SheetXmlRepository.ToXml(otherSpriteSheet);
            if (mySpriteSheetXml.ToString() != otherSpriteSheetXml.ToString()) Debugger.Break();
        }

        private object CreateMouseCursor()
        {
            return new MouseCursor(this.InputConfiguration.AddMouseTracking(this.Camera));
        }

        private ILayer CreateMouseFollow()
        {
            var layer = new SpriteLayer("MouseFollow");
            layer.CameraMode = CameraMode.Fix;

            var linkSheet = this.ResourceManager.GetSpriteSheet("Link");
            this.linkMouseFollow = new Sprite(linkSheet, "Link01");
            this.linkMouseFollow.Origin = new Vector(8, 11);

            layer.AddSprite(this.linkMouseFollow);

            return layer;
        }

        private DiagnosticHud CreateDiagnosticLayer()
        {
            var font = this.ResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");

            var config = new DiagnosticHudConfiguration();
            config.EnableCameraTracking(this.Camera);
            config.EnableMouseTracking(this.InputConfiguration.AddMouseTracking(this.Camera));
            config.EnableHitTracking(() => this.hits);
            config.AddLine("Range: {0:f1}", () => this.range);
            config.AddLine("Objects: {0} (drawn: {1})",
                () => this.layer.TotalElements, () => this.layer.DrawnElementsLastFrame);

            this.diagnosticHud = new DiagnosticHud(font, config);

            return this.diagnosticHud;
        }

        private ImageLayer CreateImageLayer()
        {
            var texture = this.ResourceManager.GetTexture(@"Sandbox\LinkSheet");

            return new ImageLayer("Image", texture, new Rectangle(10, 10, 250, 250));
        }

        private ColorLayer CreateColorLayer()
        {
            var alpha = Math.Min(this.range, 1.0f);

            return new ColorLayer("Red", new Color(255, 0, 0, (int)(255 * alpha)));
        }

        private HexLayer CreateHexLayer()
        {
            var texture = this.ResourceManager.GetTexture(@"Sandbox\HexSheet");
            var sheet = new HexSheet(texture, "Hexes", new Size(68, 60));
            var red = sheet.CreateHexDefinition("red", new Point(55, 30));
            var yellow = sheet.CreateHexDefinition("yellow", new Point(163, 330));
            var purple = sheet.CreateHexDefinition("purple", new Point(488, 330));

            //sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color HexSheet.xml");

            this.ResourceManager.AddHexSheet(sheet);

            var hexLayer = new HexLayer("Hex", new Size(4, 4), new Size(68, 60), 42);
            hexLayer[2, 0] = purple;
            hexLayer[2, 1] = purple;
            hexLayer[2, 2] = yellow;
            hexLayer[0, 1] = red;
            hexLayer[1, 1] = red;

            hexLayer.ParallaxScrollingVector = new Vector(4.0f, 0.5f);
            hexLayer.Offset = new Vector(-25, 0);

            return hexLayer;
        }

        private TileLayer CreateTileLayer()
        {
            var texture = this.ResourceManager.GetTexture(@"Sandbox\TileSheet");
            var sheet = new TileSheet(texture, "Background", new Size(16, 16));
            var red = sheet.CreateTileDefinition("red", new Point(0, 0));
            var green = sheet.CreateTileDefinition("green", new Point(16, 0));
            sheet.CreateTileDefinition("yellow", new Point(32, 0));
            var purple = sheet.CreateTileDefinition("purple", new Point(0, 16));
            sheet.CreateTileDefinition("orange", new Point(16, 16));
            sheet.CreateTileDefinition("blue", new Point(32, 16));

            //sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Color TileSheet.xml");

            this.ResourceManager.AddTileSheet(sheet);

            var tileLayer = new TileLayer("Tiles", new Size(32, 32), new Size(16, 16));
            tileLayer[0, 0] = purple;
            tileLayer[1, 1] = red;
            tileLayer[10, 10] = purple;
            tileLayer[4, 20] = green;

            tileLayer.ParallaxScrollingVector = new Vector(2.0f, 2.0f);
            tileLayer.Offset = new Vector(0, -20);

            return tileLayer;
        }

        private SpriteLayer CreateSpriteLayer()
        {
            var texture = this.ResourceManager.GetTexture(@"Sandbox\LinkSheet");
            var sheet = new SpriteSheet(texture, "Link");
            sheet.CreateSpriteDefinition("Link01", new Rectangle(3, 3, 16, 22));
            sheet.CreateSpriteDefinition("Sleep01", new Rectangle(45, 219, 32, 40));

            //sheet.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\Link SpriteSheet.xml");

            var link01 = new Sprite(sheet, "Link01") { Position = this.player };
            var sleep01 = new Sprite(sheet, "Sleep01") { Position = new Vector(125, 25) };

            this.ResourceManager.AddSpriteSheet(sheet);

            var spriteLayer = new SpriteLayer("Sprites");
            spriteLayer.AddSprite(link01);
            spriteLayer.AddSprite(sleep01);
            spriteLayer.ParallaxScrollingVector = new Vector(4.0f, 8.0f);
            spriteLayer.Offset = new Vector(50, 50);

            return spriteLayer;
        }

        private DrawingLayer CreateHexLayerTestDistance()
        {
            var layer = new DrawingLayer("Hex drawing test");
            var font = this.ResourceManager.GetDrawingFont(@"Sandbox\SpriteFont1");
            var hexMap = HexGrid.CreateHexMap(30, 9);
            foreach (var hex in hexMap.Hexes)
            {
                var distance = HexGrid.HexDistance(hexMap[4, 5], hex);

                var color = distance == 1
                    ? new Color(0, 255, 0, 255)
                    : distance == 2
                        ? new Color(0, 192, 0, 255)
                        : distance == 3
                            ? new Color(0, 128, 0, 255)
                            : distance == 4
                                ? new Color(0, 64, 0, 255)
                                : distance == 5
                                    ? new Color(128, 0, 128, 255)
                                    : Color.Red;

                layer.AddPolygone(3, color, hex.GetVertices());

                var text = string.Format("{0},{1}", hex.Position.X - 4, hex.Position.Y - 5 + (hex.Position.X % 2) * .5);
                var measure = font.MeasureString(text);

                layer.AddText(font, text, hex.Center - (measure / 2.0f), Color.Yellow);
            }

            this.layer = layer;

            return layer;
        }
    }
}
