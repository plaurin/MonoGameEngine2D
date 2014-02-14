using System.Collections;

namespace GameFramework
{
    public interface IUpdatable
    {
        void Update(IGameTiming gameTiming);
    }
}