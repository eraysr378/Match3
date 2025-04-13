using System;

namespace Interfaces
{
    public interface IRainbowHittable
    {
        public event Action OnRainbowHitHandled;
        void OnHitByRainbow();
    }
}