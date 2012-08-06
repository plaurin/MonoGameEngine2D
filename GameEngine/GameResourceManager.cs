using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using WindowsGame1.Hexes;

namespace WindowsGame1
{
    public class GameResourceManager
    {
        private readonly ContentManager contentManager;

        private readonly Dictionary<string, Texture2D> textureDictionary;

        private readonly Dictionary<string, HexSheet> hexSheetDictionary;

        public GameResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;

            this.textureDictionary = new Dictionary<string, Texture2D>();
            this.hexSheetDictionary = new Dictionary<string, HexSheet>();

            this.AddHexSheet(NullHexDefinition.CreateInstance().Sheet);
        }

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

        public void AddHexSheet(HexSheet sheet)
        {
            this.hexSheetDictionary.Add(sheet.Name, sheet);
        }

        public HexSheet GetHexSheet(string name)
        {
            return this.hexSheetDictionary[name];
        }
    }
}