using System.Collections.Generic;

namespace GameFramework.Sprites
{
    public class SpriteCompositeTemplate : ISpriteTemplate
    {
        private readonly string name;
        private readonly List<TemplateDefinition> templateDefinitions;

        public SpriteCompositeTemplate(string name)
        {
            this.name = name;
            this.templateDefinitions = new List<TemplateDefinition>();
        }

        public SpriteCompositeTemplate AddTemplate(ISpriteTemplate template, Vector? offset = null)
        {
            var def = new TemplateDefinition
            {
                Template = template, 
                Offset = offset.HasValue ? offset.Value : Vector.Zero
            };

            this.templateDefinitions.Add(def);
            return this;
        }

        public SpriteBase CreateInstance()
        {
            var sprites = new List<SpriteBase>();
            foreach (var templateDef in this.templateDefinitions)
            {
                var sprite = templateDef.Template.CreateInstance();
                sprite.Position = templateDef.Offset;
                sprites.Add(sprite);
            }

            return new SpriteComposite(this.name, sprites);
        }

        private struct TemplateDefinition
        {
            public ISpriteTemplate Template { get; set; }

            public Vector Offset { get; set; }
        }
    }
}