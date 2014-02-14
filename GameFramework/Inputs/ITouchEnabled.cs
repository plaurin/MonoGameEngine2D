using System.Collections.Generic;

namespace GameFramework.Inputs
{
    public interface ITouchEnabled
    {
        IEnumerable<TouchGestureType> TouchGestures { get; }
    }
}