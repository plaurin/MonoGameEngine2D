using System;
using System.Collections.Generic;

namespace GameFramework.Sprites
{
    public class SpriteCompositeTemplate : ISpriteTemplate, IComposite, INavigatorMetadataProvider
    {
        private readonly string name;

        private readonly List<TemplateDefinition> templateDefinitions;

        internal SpriteCompositeTemplate(string name)
        {
            this.name = name;
            this.templateDefinitions = new List<TemplateDefinition>();
        }

        public IEnumerable<object> Children
        {
            get { return this.templateDefinitions; }
        }

        public SpriteCompositeTemplate AddTemplate(ISpriteTemplate template, SpriteTransform transform = null)
        {
            return this.AddTemplate(transform, template);
        }

        public SpriteCompositeTemplate AddTemplate(SpriteTransform transform, ISpriteTemplate template)
        {
            var def = new TemplateDefinition
            {
                Template = template,
                Transform = transform
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
                if (templateDef.Transform != null)
                {
                    sprite.Position = templateDef.Transform.Translation;
                    sprite.Rotation = templateDef.Transform.Rotation;
                    sprite.Scale = templateDef.Transform.Scale;
                    sprite.Color = templateDef.Transform.Color;
                }

                sprites.Add(sprite);
            }

            return new SpriteComposite(this.name, sprites);
        }

        public NavigatorMetadata GetMetadata()
        {
            // TODO get new kind and icon
            return new NavigatorMetadata(this.name, NodeKind.Entity);
        }

        private class TemplateDefinition : IComposite, INavigatorMetadataProvider
        {
            public ISpriteTemplate Template { get; set; }

            public SpriteTransform Transform { get; set; }

            public IEnumerable<object> Children
            {
                get { return new[] { this.Template }; }
            }

            public NavigatorMetadata GetMetadata()
            {
                return new NavigatorMetadata("Template", NodeKind.Entity);
            }
        }
    }
}