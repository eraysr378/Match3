using System;

namespace Interfaces
{
    public interface IRainbowHittable
    {
        bool TryHandleRainbowHit(Action onHandled);
    }
}