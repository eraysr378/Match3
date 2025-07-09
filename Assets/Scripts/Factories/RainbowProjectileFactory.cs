using Factories.BaseFactories;
using Projectiles;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "RainbowProjectileFactory",
        menuName = "Factories/RainbowProjectileFactory")]
    public class RainbowProjectileFactory : BasePoolableObjectFactory<RainbowProjectile>
    {
        
    }
}