// Modified for MonoGameEngine2D by Pascal Laurin @2014
// Originals can be found at https://github.com/marshallward/TiledSharp
// Original notice below:
// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GameFramework.IO.TiledSharp
{
    internal class TmxList<T> : KeyedCollection<string, T> where T : ITmxElement
    {
        private static readonly Dictionary<Tuple<TmxList<T>, string>, int> NameCount = new Dictionary<Tuple<TmxList<T>, string>, int>();

        public new void Add(T t)
        {
            // Rename duplicate entries by appending a number
            var key = Tuple.Create(this, t.Name);
            if (this.Contains(t.Name))
                NameCount[key] += 1;
            else
                NameCount.Add(key, 0);
            base.Add(t);
        }

        protected override string GetKeyForItem(T t)
        {
            var key = Tuple.Create(this, t.Name);
            var count = NameCount[key];

            var dupes = 0;
            var itemKey = t.Name;

            // For duplicate keys, append a counter
            // For pathological cases, insert underscores to ensure uniqueness
            while (this.Contains(itemKey))
            {
                itemKey = t.Name + string.Concat(Enumerable.Repeat("_", dupes)) + count;
                dupes++;
            }

            return itemKey;
        }
    }
}