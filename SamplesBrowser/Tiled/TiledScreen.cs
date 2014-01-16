using System;
using System.Diagnostics;
using System.Linq;
using GameFramework;
using GameFramework.Cameras;
using GameFramework.Drawing;
using GameFramework.Hexes;
using GameFramework.Inputs;
using GameFramework.IO;
using GameFramework.IO.TiledSharp;
using GameFramework.Scenes;
using GameFramework.Screens;
using GameFramework.Sheets;
using GameFramework.Sprites;
using GameFramework.Tiles;

namespace SamplesBrowser.Tiled
{
    public class TiledScreen : ScreenBase
    {
        private readonly ScreenNavigation screenNavigation;

        private Camera camera;

        private GameResourceManager gameResourceManager;

        private Point player;

        private float range;

        private Scene scene;

        private InputConfiguration inputConfiguration;

        public TiledScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        public override void Initialize(Camera theCamera)
        {
            this.camera = theCamera;
            this.player = new Point(25, 25);
            this.range = 0.25f;

            theCamera.Center = CameraCenter.WindowTopLeft;
        }

        public override InputConfiguration GetInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            this.inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

            //this.inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left).MapTo(gt => this.camera.Move(-60 * gt.ElapsedSeconds, 0));
            //this.inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right).MapTo(gt => this.camera.Move(60 * gt.ElapsedSeconds, 0));
            //this.inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up).MapTo(gt => this.camera.Move(0, -60 * gt.ElapsedSeconds));
            //this.inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down).MapTo(gt => this.camera.Move(0, 60 * gt.ElapsedSeconds));

            //this.inputConfiguration.AddDigitalButton("ZoomIn").Assign(KeyboardKeys.A).MapTo(gt => this.camera.ZoomFactor *= 1.2f * (1 + gt.ElapsedSeconds));
            //this.inputConfiguration.AddDigitalButton("ZoomOut").Assign(KeyboardKeys.Z).MapTo(gt => this.camera.ZoomFactor *= 1 / (1.2f * (1 + gt.ElapsedSeconds)));

            //this.inputConfiguration.AddDigitalButton("RangeUp").Assign(KeyboardKeys.W).MapTo(gt => this.range *= 1.2f * (1 + gt.ElapsedSeconds));
            //this.inputConfiguration.AddDigitalButton("RangeDown").Assign(KeyboardKeys.Q).MapTo(gt => this.range *= 1 / (1.2f * (1 + gt.ElapsedSeconds)));

            //this.inputConfiguration.AddDigitalButton("Selection").Assign(MouseButtons.Left).MapTo(elapse =>
            //{
            //    this.hits = string.Join("; ", this.scene.GetHits(this.mouseState.AbsolutePosition, this.camera));
            //});

            //this.inputConfiguration.AddMouseTracking(this.camera).OnMove((mt, e) => this.mouseState = mt);

            return this.inputConfiguration;
        }

        public override void LoadContent(GameResourceManager theResourceManager)
        {
            this.gameResourceManager = theResourceManager;

            //this.gameResourceManager.AddDrawingFont(@"Sandbox\SpriteFont1");
        }

        public override void Update(IGameTiming gameTime)
        {
            //var colorMap = this.scene.Maps.OfType<ColorMap>().FirstOrDefault();
            //if (colorMap != null)
            //    colorMap.Color = new Color(255, 0, 0, (int)(255 * Math.Min(this.range, 1.0f)));

            //this.fpsElement.SetParameters(gameTime.Fps);
            //this.viewPortElement.SetParameters(this.camera.SceneViewPort);
            //this.translationElement.SetParameters(this.camera.SceneTranslationVector);
            //this.positionElement.SetParameters(this.camera.Position);
            //this.zoomingElement.SetParameters(this.camera.ZoomFactor);
            //this.rangeElement.SetParameters(this.range);
            //this.mouseElement.SetParameters(this.mouseState);
            //this.mouseElement2.SetParameters(this.mouseState.AbsolutePosition);
            //this.hitElement.SetParameters(this.hits);

            //this.mouseMap.ClearAll();
            //this.mouseMap.AddLine(this.mouseState.AbsolutePosition.Translate(-10, 0).ToVector(),
            //    this.mouseState.AbsolutePosition.Translate(10, 0).ToVector(), 2, Color.Red);

            //this.mouseMap.AddLine(this.mouseState.AbsolutePosition.Translate(0, -10).ToVector(),
            //    this.mouseState.AbsolutePosition.Translate(0, 10).ToVector(), 2, Color.Red);
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("Tiled");

            //var map = new TmxMap(@"Content\Tiled\untitled.tmx");

            //var tmxLayer = map.Layers[0];
            //var tmxTileset = map.Tilesets[0];
            //this.scene.AddMap(this.CreateTileMap(map, tmxTileset, tmxLayer));

            foreach (var tileMap in TiledHelper.LoadFile(@"Tiled\untitled.tmx", this.gameResourceManager))
                this.scene.AddLayer(tileMap);

            //this.scene.AddMap(this.CreateImageMap());
            //this.scene.AddMap(this.CreateHexMap());
            //this.scene.AddMap(this.CreateTileMap());
            //this.scene.AddMap(this.CreateColorMap());
            //this.scene.AddMap(this.CreateHexMapTestDistance());
            //this.scene.AddMap(this.CreateSpriteMap());
            //this.scene.AddMap(this.CreateDiagnosticText());
            //this.scene.AddMap(this.CreateMouseCursorMap());

            //this.scene.Save(@"C:\Users\Pascal\Dev\DotNet\GitHub\XNAGameEngine2D\TestScene.xml");

            //this.TestSceneSaveLoad();
            //this.TestSheetsSaveLoad();

            return this.scene;
        }

        //private MapBase CreateTileMap(TmxMap tmxMap, TmxTileset tmxTileset, TmxLayer tmxLayer)
        //{
        //    //tmxMap.Tilesets[0].Image
        //    var texture = this.gameResourceManager.GetTexture(@"Tiled\tmw_desert_spacing");
        //    var sheet = new TileSheet(texture, "Desert", new Size(tmxMap.TileWidth, tmxMap.TileHeight));

        //    var numTileWidth = (tmxTileset.Image.Width - (2 * tmxTileset.Margin) + tmxTileset.Spacing) / (tmxTileset.TileWidth + tmxTileset.Spacing);
        //    var numTileHeight = (tmxTileset.Image.Height - (2 * tmxTileset.Margin) + tmxTileset.Spacing) / (tmxTileset.TileHeight + tmxTileset.Spacing);

        //    for (int j = 0; j < numTileHeight; j++)
        //        for (int i = 0; i < numTileWidth; i++)
        //        {
        //            sheet.CreateTileDefinition((tmxTileset.FirstGid + i + numTileWidth * j).ToString(),
        //                new Point(tmxTileset.Margin + i * (tmxTileset.TileWidth + tmxTileset.Spacing),
        //                    tmxTileset.Margin + j * (tmxTileset.TileHeight + tmxTileset.Spacing)));
        //        }

        //    this.gameResourceManager.AddTileSheet(sheet);

        //    var tileMap = new TileMap(tmxLayer.Name, new Size(tmxMap.Width, tmxMap.Height), new Size(tmxMap.TileWidth, tmxMap.TileHeight));

        //    foreach (var tile in tmxLayer.Tiles)
        //    {
        //        if (tile.Gid != 0)
        //            tileMap[tile.X, tile.Y] = sheet.Definitions[tile.Gid.ToString()];
        //    }

        //    return tileMap;
        //}
    }
}
