using System;

namespace GameFramework.Sheets
{
    public abstract class SheetBase
    {
        protected SheetBase(Texture texture, string name)
        {
            this.Texture = texture;
            this.Name = name;
        }

        public Texture Texture { get; private set; }

        public string Name { get; private set; }

        //[Obsolete]
        //protected abstract IEnumerable<object> GetXml();
    }
}