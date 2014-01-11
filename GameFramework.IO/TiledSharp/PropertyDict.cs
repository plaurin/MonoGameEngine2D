// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Collections.Generic;
using System.Xml.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class PropertyDict : Dictionary<string, string>
    {
        internal PropertyDict(XElement xmlProp)
        {
            if (xmlProp == null) return;

            foreach (var p in xmlProp.Elements("property"))
            {
                var pname = p.Attribute("name").Value;
                var pval = p.Attribute("value").Value;
                this.Add(pname, pval);
            }
        }
    }
}