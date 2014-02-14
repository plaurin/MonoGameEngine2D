using System.Collections.Generic;

namespace GameFramework
{
    public interface IComposite
    {
        IEnumerable<object> Children { get; }
    }
}