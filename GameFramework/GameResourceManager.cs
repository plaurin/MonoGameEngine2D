using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Drawing;
using GameFramework.Hexes;
using GameFramework.Sprites;
using GameFramework.Tiles;

namespace GameFramework
{
    public class GameResourceManager : IComposite, INavigatorMetadataProvider
    {
        #region Fields

        private readonly Dictionary<string, Texture> textureDictionary;

        private readonly Dictionary<string, HexSheet> hexSheetDictionary;

        private readonly Dictionary<string, TileSheet> tileSheetDictionary;

        private readonly Dictionary<string, SpriteSheet> spriteSheetDictionary;

        private readonly Dictionary<string, DrawingFont> drawingFontDictionary;
        
        #endregion

        #region Constructor

        public GameResourceManager()
        {
            this.textureDictionary = new Dictionary<string, Texture>();
            this.hexSheetDictionary = new Dictionary<string, HexSheet>();
            this.tileSheetDictionary = new Dictionary<string, TileSheet>();
            this.spriteSheetDictionary = new Dictionary<string, SpriteSheet>();
            this.drawingFontDictionary = new Dictionary<string, DrawingFont>();

            this.AddHexSheet(NullHexDefinition.CreateInstance().Sheet);
            this.AddTileSheet(NullTileDefinition.CreateInstance().Sheet);
        }

        #endregion

        public IEnumerable<object> Children
        {
            get
            {
                return this.drawingFontDictionary.Values.Cast<object>()
                    .Concat(this.textureDictionary.Values)
                    .Concat(this.tileSheetDictionary.Values)
                    .Concat(this.hexSheetDictionary.Values)
                    .Concat(this.spriteSheetDictionary.Values);
            }
        }

        public NavigatorMetadata GetMetadata()
        {
            // TODO: Need new node kind and label
            return new NavigatorMetadata("Resource Manager", NodeKind.Utility);
        }

        #region Texture

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
            Texture texture;

            if (!this.textureDictionary.TryGetValue(assetName, out texture))
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
            TileSheet result;
            return this.tileSheetDictionary.TryGetValue(name, out result) ? result : null;
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

        public virtual DrawingFont GetDrawingFont(string assetName)
        {
            DrawingFont drawingFont;

            if (!this.drawingFontDictionary.TryGetValue(assetName, out drawingFont))
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
    }
}