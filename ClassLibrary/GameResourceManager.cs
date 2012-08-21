using System;
using System.Collections.Generic;

using ClassLibrary.Drawing;
using ClassLibrary.Hexes;
using ClassLibrary.Scenes;
using ClassLibrary.Sprites;
using ClassLibrary.Tiles;

namespace ClassLibrary
{
    public class GameResourceManager
    {
        #region Fields

        //private readonly ContentManager contentManager;

        private readonly Dictionary<string, Texture> textureDictionary;

        private readonly Dictionary<string, HexSheet> hexSheetDictionary;

        private readonly Dictionary<string, TileSheet> tileSheetDictionary;

        private readonly Dictionary<string, SpriteSheet> spriteSheetDictionary;

        private readonly Dictionary<string, DrawingFont> drawingFontDictionary;
        
        private readonly Dictionary<string, Scene> sceneDictionary;

        #endregion

        #region Constructor

        //public GameResourceManager(ContentManager contentManager)
        //{
        //    this.contentManager = contentManager;
        public GameResourceManager()
        {
            this.textureDictionary = new Dictionary<string, Texture>();
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

        //protected Dictionary<string, Texture> TextureDictionary
        //{
        //    get
        //    {
        //        return this.textureDictionary;
        //    }
        //}

        public void AddTexture(Texture texture)
        {
            this.textureDictionary.Add(texture.Name, texture);
        }

        public void AddTexture(string assetName)
        {
            this.textureDictionary.Add(assetName, null);
        }

        public void AddTexture(string assetName, Texture texture)
        {
            this.textureDictionary.Add(assetName, texture);
            texture.Name = assetName;
        }

        public Texture GetTexture(string assetName)
        {
            var texture = this.textureDictionary[assetName];

            if (texture == null)
            {
                texture = this.CreateTexture(assetName);
                this.textureDictionary[assetName] = texture;
            }

            return texture;
        }

        protected virtual Texture CreateTexture(string assetName)
        {
            throw new NotSupportedException();
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

        public virtual DrawingFont GetDrawingFont(string assetName)
        {
            var drawingFont = this.drawingFontDictionary[assetName];

            if (drawingFont == null)
            {
                drawingFont = this.CreateDrawingFont(assetName);
                this.drawingFontDictionary[assetName] = drawingFont;
            }

            return drawingFont;
        }

        protected virtual DrawingFont CreateDrawingFont(string assetName)
        {
            return new DrawingFont
            {
                Name = assetName,
                //Font = this.contentManager.Load<SpriteFont>(assetName)
            };
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