using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Drawing;
using WindowsGame1.Hexes;
using WindowsGame1.Scenes;
using WindowsGame1.Sprites;
using WindowsGame1.Tiles;

namespace WindowsGame1
{
    public class GameResourceManager
    {
        #region Fields

        private readonly ContentManager contentManager;

        private readonly Dictionary<string, Texture2D> textureDictionary;

        private readonly Dictionary<string, HexSheet> hexSheetDictionary;

        private readonly Dictionary<string, TileSheet> tileSheetDictionary;

        private readonly Dictionary<string, SpriteSheet> spriteSheetDictionary;

        private readonly Dictionary<string, DrawingFont> drawingFontDictionary;
        
        private readonly Dictionary<string, Scene> sceneDictionary;

        #endregion

        #region Constructor

        public GameResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;

            this.textureDictionary = new Dictionary<string, Texture2D>();
            this.hexSheetDictionary = new Dictionary<string, HexSheet>();
            this.tileSheetDictionary = new Dictionary<string, TileSheet>();
            this.spriteSheetDictionary = new Dictionary<string, SpriteSheet>();
            this.drawingFontDictionary = new Dictionary<string, DrawingFont>();
            this.sceneDictionary = new Dictionary<string, Scene>();

            this.AddHexSheet(NullHexDefinition.CreateInstance().Sheet);
            this.AddTileSheet(NullTileDefinition.CreateInstance().Sheet);
        }

        #endregion

        #region Texture

        public void AddTexture(string assetName)
        {
            this.textureDictionary.Add(assetName, null);
        }

        public void AddTexture(string assetName, Texture2D texture)
        {
            this.textureDictionary.Add(assetName, texture);
            texture.Name = assetName;
        }

        public Texture2D GetTexture(string assetName)
        {
            var texture = this.textureDictionary[assetName];

            if (texture == null)
            {
                texture = this.contentManager.Load<Texture2D>(assetName);
                texture.Name = assetName;
                this.textureDictionary[assetName] = texture;
            }

            return texture;
        }

        #endregion

        #region HexSheet

        public void AddHexSheet(HexSheet sheet)
        {
            this.hexSheetDictionary.Add(sheet.Name, sheet);
        }

        public HexSheet GetHexSheet(string name)
        {
            return this.hexSheetDictionary[name];
        }

        #endregion

        #region TileSheet

        public void AddTileSheet(TileSheet sheet)
        {
            this.tileSheetDictionary.Add(sheet.Name, sheet);
        }

        public TileSheet GetTileSheet(string name)
        {
            return this.tileSheetDictionary[name];
        }

        #endregion

        #region SpriteSheet

        public void AddSpriteSheet(SpriteSheet sheet)
        {
            this.spriteSheetDictionary.Add(sheet.Name, sheet);
        }

        public SpriteSheet GetSpriteSheet(string name)
        {
            return this.spriteSheetDictionary[name];
        }

        #endregion

        #region SpriteFont

        public void AddDrawingFont(string assetName)
        {
            this.drawingFontDictionary.Add(assetName, null);
        }

        public DrawingFont GetDrawingFont(string assetName)
        {
            var drawingFont = this.drawingFontDictionary[assetName];

            if (drawingFont == null)
            {
                drawingFont = new DrawingFont
                {
                    Name = assetName,
                    Font = this.contentManager.Load<SpriteFont>(assetName)
                };
                this.drawingFontDictionary[assetName] = drawingFont;
            }

            return drawingFont;
        }

        #endregion

        #region Scene

        public void AddScene(Scene scene)
        {
            this.sceneDictionary.Add(scene.Name, scene);
        }

        public Scene GetScene(string sceneName)
        {
            return this.sceneDictionary[sceneName];
        }

        #endregion
    }
}